namespace DevKit.UI.MVVM
{
    public abstract class CanvasAttach : CanvasView, IAttachableCanvasView
    {
        public abstract int Order { get; }
    }
    
    public abstract class CanvasAttach<T> : CanvasView<T>, IAttachableCanvasView  where T : IViewModel
    {
        public abstract int Order { get; }
    }
}