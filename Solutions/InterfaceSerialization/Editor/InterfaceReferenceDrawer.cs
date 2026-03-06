#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DevKit.InterfaceSerialization
{
    [CustomPropertyDrawer(typeof(InterfaceReference<>))]
    [CustomPropertyDrawer(typeof(InterfaceReference<,>))]
    public class InterfaceReferenceDrawer : PropertyDrawer
    {
        const string UnderlyingValueFieldName = "_underlyingValue";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var underlyingProperty = property.FindPropertyRelative(UnderlyingValueFieldName);
            var assignedObject = underlyingProperty.objectReferenceValue;
            var arguments = GetArguments(fieldInfo);
            var interfaceName = arguments.InterfaceType.Name;

            var objectField = new ObjectField()
            {
                label = property.displayName,
                tooltip = interfaceName,
                objectType = arguments.InterfaceType,
                allowSceneObjects = true
            };
            objectField.AddToClassList("unity-base-field__aligned");

            objectField.SetValueWithoutNotify(assignedObject);
            objectField.RegisterValueChangedCallback(evt =>
            {
                var newAssignedObject = evt.newValue;

                if (newAssignedObject != null)
                {
                    Object component = null;

                    if (newAssignedObject is GameObject gameObject)
                        component = gameObject.GetComponent(arguments.InterfaceType);
                    else if (arguments.InterfaceType.IsAssignableFrom(newAssignedObject.GetType()))
                        component = newAssignedObject;

                    if (component != null)
                    {
                        ValidateAndAssignObject(underlyingProperty, component, component.name, interfaceName);
                    }
                    else
                    {
                        Debug.LogWarning($"Assigned object doesn't implement required interface '{interfaceName}'.");
                        underlyingProperty.objectReferenceValue = null;
                    }
                }
                else
                {
                    underlyingProperty.objectReferenceValue = null;
                }

                underlyingProperty.serializedObject.ApplyModifiedProperties();
                objectField.SetValueWithoutNotify(underlyingProperty.objectReferenceValue);
            });

            var container = new VisualElement();
            container.Add(objectField);

            return container;
        }

        private static InterfaceArguments GetArguments(FieldInfo fieldInfo)
        {
            var fieldType = fieldInfo.FieldType;

            if (!TryGetValuesFromInterfaceReference(fieldType, out Type interfaceType, out Type objectType))
                GetTypesFromList(fieldType, out interfaceType, out objectType);

            return new InterfaceArguments(interfaceType, objectType);
        }

        private static bool TryGetValuesFromInterfaceReference(Type type, out Type resultInterfaceType, out Type resultObjectType)
        {
            resultInterfaceType = resultObjectType = null;

            if (!type.IsGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();

            if (genericType == typeof(InterfaceReference<>))
                type = type.BaseType;

            if (type != null && type.GetGenericTypeDefinition() == typeof(InterfaceReference<,>))
            {
                var types = type.GetGenericArguments();

                resultInterfaceType = types[0];
                resultObjectType = types[1];

                return true;
            }

            return false;
        }

        private static void GetTypesFromList(Type type, out Type resultInterfaceType, out Type resultObjectType)
        {
            resultInterfaceType = resultObjectType = null;

            var listInterface = type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));

            if (listInterface != null)
            {
                var elementType = listInterface.GetGenericArguments()[0];
                TryGetValuesFromInterfaceReference(elementType, out resultInterfaceType, out resultObjectType);
            }
        }

        private static void ValidateAndAssignObject(SerializedProperty property, Object targetObject, string componentNameOrType, string interfaceName = null)
        {
            if (targetObject != null)
            {
                property.objectReferenceValue = targetObject;
            }
            else
            {
                var message = interfaceName == null ? "assigned object" : $"Gameobject '{componentNameOrType}'";
                Debug.LogWarning($"The {message} doesn't have a conponent that implements '{interfaceName}'");
                property.objectReferenceValue = null;
            }
        }
    }

    public struct InterfaceArguments
    {
        public readonly Type InterfaceType;
        public readonly Type ObjectType;

        public InterfaceArguments(Type interfaceType, Type objectType)
        {
            Debug.Assert(typeof(Object).IsAssignableFrom(objectType),
                $"{nameof(ObjectType)} should be of type {typeof(Object)}");

            Debug.Assert(interfaceType.IsInterface, $"{nameof(interfaceType)} should be an interface");

            InterfaceType = interfaceType;
            ObjectType = objectType;
        }
    }
}
#endif