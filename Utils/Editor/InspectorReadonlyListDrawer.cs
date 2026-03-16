#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace DevKit.Utils.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadonlyList<>))]
    public class InspectorReadonlyListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            var listProperty = property.FindPropertyRelative("_lockedItems");
            if (listProperty == null || !listProperty.isArray)
            {
                var errorLabel = new Label("List property '_lockedItems' not found");
                root.Add(errorLabel);
                return root;
            }

            var listView = new ListView
            {
                headerTitle = property.displayName,
                showFoldoutHeader = true,
                showAddRemoveFooter = false,
                showBorder = true,
                reorderable = false,
                selectionType = SelectionType.None,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                fixedItemHeight = 24,
                showBoundCollectionSize = true,
                itemsSource = GetItemsSource(listProperty),

                makeItem = () => new PropertyField(),
                bindItem = (element, index) =>
                {
                    if (element is PropertyField propertyField && listProperty.arraySize > index)
                    {
                        var elementProp = listProperty.GetArrayElementAtIndex(index);
                        propertyField.BindProperty(elementProp);
                        propertyField.SetEnabled(true);
                    }
                }
            };

            listView.Q<TextField>(name: BaseListView.arraySizeFieldUssClassName)?.SetEnabled(false);

            root.TrackPropertyValue(listProperty, _ =>
            {
                listView.itemsSource = GetItemsSource(listProperty);
                // listView.Rebuild();
            });

            root.Add(listView);
            
            return root;
        }

        private List<SerializedProperty> GetItemsSource(SerializedProperty listProperty)
        {
            var items = new List<SerializedProperty>();
            for (var i = 0; i < listProperty.arraySize; i++)
                items.Add(listProperty.GetArrayElementAtIndex(i));

            return items;
        }
    }
}
#endif