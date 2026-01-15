using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public class RawToolkitViewDecorator : IToolkitView
    {
        private readonly VisualTreeAsset _uiAsset;
        private readonly IToolkitView _childView;
        private VisualElement _root;
        private VisualElement _childRoot;

        public RawToolkitViewDecorator(VisualTreeAsset asset, IToolkitView childView)
        {
            _childView = childView;
            _uiAsset = asset;
        }

        public virtual VisualElement InitAndGetRoot()
        {
            _root = new VisualElement();
            UxmlUtil.StyleAttachable(_root);

            var parentRoot = UxmlUtil.CloneStyled(_uiAsset);
            _root.Add(parentRoot);

            _childRoot = _childView.InitAndGetRoot();
            _root.Add(_childRoot);

            return _root;
        }

        public void Show()
        {
            UxmlUtil.Show(_root);
            _childView.Show();
        }

        public void Hide()
        {
            _childView.Hide();
            UxmlUtil.Hide(_root);
        }
    }
}