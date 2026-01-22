using UnityEngine.UIElements;

namespace DevKit.UI
{
    internal static class UxmlUtil
    {
        public static VisualElement CloneStyled(VisualTreeAsset asset)
        {
            var root = asset.CloneTree();
            root.pickingMode = PickingMode.Ignore; 
            root.style.flexGrow = 1;

            return root;
        }

        public static VisualElement StyleAttachable(VisualElement root)
        {
            var styles = root.style;
            
            styles.position = Position.Absolute;
            styles.left = 0;
            styles.right = 0;
            styles.top = 0;
            styles.bottom = 0;
            
            return root;
        }

        public static void Show(VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
        }

        public static void Hide(VisualElement element)
        {
            element.style.display = DisplayStyle.None;
        }
    }
}