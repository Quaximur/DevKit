namespace DevKit.UI.MVVM
{
    /// <summary>
    /// View for displaying in a Canvas without binding to a ViewModel.
    /// E.g., used for serialization under a common abstraction.
    /// </summary>
    public abstract class CanvasView : BaseView
    {
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// <see cref="CanvasView"/> with binding to a ViewModel.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CanvasView<T> : CanvasView, IView<T> where T : IViewModel
    {
        protected T ViewModel { get; private set; }

        public void Bind(T viewModel)
        {
            ViewModel = viewModel;
            OnBind(viewModel);
        }

        protected abstract void OnBind(T viewModel);
    }
}