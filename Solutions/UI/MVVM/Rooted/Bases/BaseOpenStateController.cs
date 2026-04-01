namespace DevKit.UI.MVVM.Bases
{
    public class BaseOpenStateController : IOpenStateController
    {
        public bool IsOpened;

        private IView _view;
        private IScreenViewModel _viewModel;

        public void Bind(IView view, IScreenViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
            _viewModel.OnOpenStateChanged += OnOpenStateChanged;
        }

        public void OnOpenStateChanged(bool isOpened)
        {
            if (IsOpened == isOpened)
                return;

            if (isOpened)
                OnOpening();
            else
                OnClosing();

            IsOpened = isOpened;
        }

        public void OnOpening()
        {
            _view.Show();
        }

        public void OnClosing()
        {
            _viewModel.CompleteClosing();
        }

        public void Dispose()
        {
            if (_viewModel != null)
                _viewModel.OnOpenStateChanged -= OnOpenStateChanged;
        }
    }
}