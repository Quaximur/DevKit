#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace DevKit.Utils.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadonlyList<>))]
    public class InspectorReadonlyListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var foldout = new Foldout { text = property.displayName };
            root.Add(foldout);

            var listProperty = property.FindPropertyRelative("_lockedItems");
            if (listProperty != null && listProperty.isArray)
            {
                for (var i = 0; i < listProperty.arraySize; i++)
                {
                    var elementProp = listProperty.GetArrayElementAtIndex(i);
                    var field = new PropertyField(elementProp);
                    field.SetEnabled(true);
                    foldout.Add(field);
                }
            }

            return root;
        }
    }
}
#endif