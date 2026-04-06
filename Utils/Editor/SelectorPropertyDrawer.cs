#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.Utils.Editor
{
    [CustomPropertyDrawer(typeof(SelectorAttribute))]
    public class SelectorPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var attr = fieldInfo.GetCustomAttribute<SelectorAttribute>();

            string[] options = Array.Empty<string>();

            
            if (attr.Options is { Length: > 0 }) // if specified options
                options = attr.Options;
            else if (attr.SourceType != null && !string.IsNullOrEmpty(attr.SourceMember))
            {
                var member = attr.SourceType.GetMember(attr.SourceMember,
                    BindingFlags.Public | BindingFlags.Static).FirstOrDefault();

                if (member is FieldInfo fi && fi.GetValue(null) is string[] arr1)
                    options = arr1;
                else if (member is PropertyInfo pi && pi.GetValue(null) is string[] arr2)
                    options = arr2;
            }

            var popup = new PopupField<string>(
                label: ObjectNames.NicifyVariableName(property.name),
                choices: options.ToList(),
                defaultIndex: Mathf.Max(0, Array.IndexOf(options, property.stringValue))
            );
            popup.AddToClassList("unity-base-field__aligned");

            popup.RegisterValueChangedCallback(evt =>
            {
                property.stringValue = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });

            return popup;
        }
    }
}
#endif