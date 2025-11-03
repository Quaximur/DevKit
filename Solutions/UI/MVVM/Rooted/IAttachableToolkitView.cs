using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IAttachableToolkitView : IToolkitView, IAttachableView
    {
        public GameObject gameObject { get; }
        protected VisualElement Root { get; }
        
        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject);
            rootUI.Attach(Root);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(Root);
            rootUI.Detach(gameObject);
        }
    }
}