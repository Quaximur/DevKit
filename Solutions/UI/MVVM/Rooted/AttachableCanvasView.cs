namespace DevKit.UI.MVVM
{
    public abstract class AttachableCanvasView : CanvasView, IAttachableCanvasView
    {
        public abstract int Order { get; }
    }
    
    public abstract class AttachableCanvasView<T> : CanvasView<T>, IAttachableCanvasView  where T : IViewModel
    {
        public abstract int Order { get; }
    }
}