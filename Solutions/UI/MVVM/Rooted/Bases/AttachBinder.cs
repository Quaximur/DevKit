using R3;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachBinder<TView, TViewModel> : AttachBinder<TView, TViewModel, TViewModel>
        where TView : BaseView, IScreenAttach<TViewModel>
        where TViewModel : IScreenViewModel
    {
        protected AttachBinder(IRootUIBinder rootUIBinder, TView view) : base(rootUIBinder, view)
        {
        }
    }

    public abstract class AttachBinder<TView, TViewModel, AViewModel> : BaseBinder<TView, TViewModel, AViewModel>
        where TView : BaseView, IScreenAttach<AViewModel>
        where TViewModel : AViewModel
        where AViewModel : IScreenViewModel
    {
        protected readonly IRootUIBinder _rootUIBinder;

        public AttachBinder(IRootUIBinder rootUIBinder, TView viewPrefab) : base(viewPrefab)
        {
            _rootUIBinder = rootUIBinder;
        }

        protected abstract TViewModel GetViewModel();

        public override AViewModel Open()
        {
            if (_currentViewModel != null) // if already exists
                return _currentViewModel;

            _currentViewModel = GetViewModel();

            var viewInstance = UnityEngine.Object.Instantiate(_view);
            viewInstance.Init();
            viewInstance.Bind(_currentViewModel);

            _currentViewModel.OnClosingCompleted.Take(1).Subscribe(_ => _rootUIBinder.ClearView(viewInstance));
            viewInstance.OnDisposed.Take(1).Subscribe(_ =>
            {
                _currentViewModel?.Dispose();
                _currentViewModel = default; // null
            });

            _rootUIBinder.AddView(viewInstance);
            _currentViewModel.Open();

            return _currentViewModel;
        }

        public override void Close()
        {
            _currentViewModel?.StartClosing();
        }
    }
}