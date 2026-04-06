using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// IToolkitView implementation without inheriting from the UnityEngine.Object
    /// </summary>
    public class RawToolkitView : IToolkitView
    {
        private readonly VisualTreeAsset _uiAsset;
        private VisualElement _root;

        public RawToolkitView(VisualTreeAsset asset)
        {
            _uiAsset = asset;
        }

        public virtual VisualElement InitAndGetRoot()
        {
            _root = UxmlUtil.CloneStyled(_uiAsset);
            return _root;
        }

        public void Show()
        {
            UxmlUtil.Show(_root);
        }

        public void Hide()
        {
            UxmlUtil.Hide(_root);
        }
    }
}