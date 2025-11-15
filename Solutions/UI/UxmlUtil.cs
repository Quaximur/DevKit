using UnityEngine.UIElements;

namespace DevKit.UI
{
    internal static class UxmlUtil
    {
        public static VisualElement CloneStyled(VisualTreeAsset asset)
        {
            var root = asset.CloneTree(); // Создаём UI из uxml, получая при  этом корневой VisualElement для данного View.
            root.pickingMode = PickingMode.Ignore; // По умолчанию корневой элемент не ловит события pointer (можно кликать насквозь).
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