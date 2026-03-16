#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DevKit.InterfaceSerialization
{
    [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        public RequireInterfaceAttribute CurrentRequireInterfaceAttribute => (RequireInterfaceAttribute)attribute;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var requiredInterfaceType = CurrentRequireInterfaceAttribute.InterfaceType;

            if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            {
                for (var i = 0; i < property.arraySize; i++)
                {
                    var elementIndex = i;
                    var propertyElement = property.GetArrayElementAtIndex(elementIndex);
                    var objectFieldElement = CreateBindedObjectField(propertyElement, requiredInterfaceType);
                    container.Add(objectFieldElement);
                }
            }
            else
            {
                var objectField = CreateBindedObjectField(property, requiredInterfaceType);
                container.Add(objectField);
            }

            return container;
        }

        private ObjectField CreateBindedObjectField(SerializedProperty property, Type interfaceType)
        {
            var objectField = CreateObjectField(property, interfaceType);

            BindObjectField(objectField, property, interfaceType);

            return objectField;
        }

        private ObjectField CreateObjectField(SerializedProperty property, Type interfaceType)
        {
            var objectField = new ObjectField()
            {
                label = property.displayName,
                tooltip = interfaceType?.Name,
                allowSceneObjects = true
            };

            return objectField;
        }

        private void BindObjectField(ObjectField objectField, SerializedProperty property, Type interfaceType)
        {
            objectField.SetValueWithoutNotify(property.objectReferenceValue);

            objectField.RegisterValueChangedCallback(e =>
            {
                SetPropertyValueWithValidation(property, e.newValue, interfaceType);
                property.serializedObject.ApplyModifiedProperties();
                objectField.SetValueWithoutNotify(property.objectReferenceValue);
            });
        }

        private void SetPropertyValueWithValidation(SerializedProperty property,
             Object newReference, Type interfaceType)
        {
            var oldReference = property.objectReferenceValue;

            if (newReference != null && newReference != oldReference)
                ValidateAndAssignObject(property, newReference, interfaceType);
            else if (newReference == null)
                property.objectReferenceValue = null;
        }

        private void ValidateAndAssignObject(SerializedProperty property,
             Object newReference, Type interfaceType)
        {
            if (newReference is GameObject gameObject)
            {
                var component = gameObject.GetComponent(interfaceType);

                if (component != null)
                {
                    property.objectReferenceValue = component;
                    return;
                }
            }
            else if (interfaceType.IsAssignableFrom(newReference.GetType()))
            {
                property.objectReferenceValue = newReference;
                return;
            }

            property.objectReferenceValue = null;
            Debug.LogWarning($"The assigned object doesn't implement '{interfaceType.Name}'");
        }
    }
}
#endif