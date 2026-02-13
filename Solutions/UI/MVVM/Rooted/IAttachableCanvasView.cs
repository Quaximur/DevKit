using UnityEngine;

namespace DevKit.UI.MVVM
{
    public interface IAttachableCanvasView : IAttachableView
    {
        protected GameObject GameObject { get; }

        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(GameObject, Priority);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(GameObject);
        }
    }
}