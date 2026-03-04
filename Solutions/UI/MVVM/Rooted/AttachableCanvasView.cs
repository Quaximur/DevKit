using UnityEngine;

namespace DevKit.UI.MVVM
{
    public abstract class AttachableCanvasView : CanvasView, IAttachableCanvasView
    {
        public virtual int CanvasSortOrder { get; }

        GameObject IAttachableCanvasView.GameObject => gameObject;
    }
    
    public abstract class AttachableCanvasView<T> : CanvasView<T>, IAttachableCanvasView  where T : IViewModel
    {
        public virtual int CanvasSortOrder { get; }

        GameObject IAttachableCanvasView.GameObject => gameObject;
    }
}