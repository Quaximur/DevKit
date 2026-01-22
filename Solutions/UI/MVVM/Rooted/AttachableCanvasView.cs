using UnityEngine;

namespace DevKit.UI.MVVM
{
    public abstract class AttachableCanvasView : CanvasView, IAttachableCanvasView
    {
        GameObject IAttachableCanvasView.GameObject => gameObject;
        
        public abstract int Priority { get; }
    }
    
    public abstract class AttachableCanvasView<T> : CanvasView<T>, IAttachableCanvasView  where T : IViewModel
    {
        GameObject IAttachableCanvasView.GameObject => gameObject;

        public abstract int Priority { get; }
    }
}