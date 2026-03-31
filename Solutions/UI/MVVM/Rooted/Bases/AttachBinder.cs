using DevKit.Utils;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachBinder<TView, TViewModel> : AttachBinder<TView, TViewModel, TViewModel>
        where TView : BaseView, IScreenAttach<TViewModel>
        where TViewModel : IScreenViewModel
    {
        protected AttachBinder(IRootUIBinder rootUIBinder) : base(rootUIBinder)
        {
        }
    }

    public abstract class AttachBinder<TView, TViewModel, AViewModel> : BaseBinder<TView, TViewModel, AViewModel>
        where TView : BaseView, IScreenAttach<AViewModel>
        where TViewModel : AViewModel
        where AViewModel : IScreenViewModel
    {
        protected readonly IRootUIBinder _rootUIBinder;

        public AttachBinder(IRootUIBinder rootUIBinder)
        {
            _rootUIBinder = rootUIBinder;
        }

        public override AViewModel Open()
        {
            if (_currentViewModel != null) // if already exists
                return _currentViewModel;

            _currentViewModel = GetViewModel();

            _view = GetView();
            _view.Init();
            _view.Bind(_currentViewModel);

            var viewNoRef = _view;

            Subs.SubscribeOnce(() => _rootUIBinder.ClearView(viewNoRef),
                x => _currentViewModel.OnClosingCompleted += x,
                y => _currentViewModel.OnClosingCompleted -= y);

            Subs.SubscribeOnce(() =>
                {
                    _currentViewModel?.Dispose();
                    _currentViewModel = default; // null
                    DisposeInstances();
                },
                x => viewNoRef.OnDisposed += x,
                x => viewNoRef.OnDisposed -= x);

            _rootUIBinder.AddView(viewNoRef);
            _currentViewModel.Open();

            return _currentViewModel;
        }

        public override void Close()
        {
            _currentViewModel?.StartClosing();
        }

        protected abstract TViewModel GetViewModel();

        protected abstract TView GetView();

        protected virtual void DisposeInstances()
        {
        }
    }
}