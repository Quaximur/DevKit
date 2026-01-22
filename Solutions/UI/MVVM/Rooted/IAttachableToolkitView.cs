using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IAttachableToolkitView : IToolkitView, IAttachableView
    {
        protected VisualElement Root { get; }
        protected GameObject GameObject { get; }
        
        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(GameObject);
            rootUI.Attach(Root);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(Root);
            rootUI.Detach(GameObject);
        }
    }
}