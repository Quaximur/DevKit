using UnityEngine;

namespace DevKit.UI.MVVM
{
    public interface IAttachableHybridView : IAttachableCanvasView, IAttachableToolkitView
    {
        protected new GameObject GameObject { get; }
        
        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(GameObject, CanvasSortOrder);
            rootUI.Attach(Root, ToolkitSortOrder);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(Root);
            rootUI.Detach(GameObject);
        }
    }
}