namespace DevKit.UI.MVVM.Bases
{
    public class OpenStateController : IOpenStateController
    {
        public bool IsOpened;

        private IOpenableView _view;
        private IScreenViewModel _viewModel;

        public void Bind(IOpenableView view, IScreenViewModel viewModel)
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
            _view.OnOpening();
        }

        public void OnClosing()
        {
            _view.OnClosing();
        }

        public void Dispose()
        {
            if (_viewModel != null)
                _viewModel.OnOpenStateChanged -= OnOpenStateChanged;
        }
    }
}