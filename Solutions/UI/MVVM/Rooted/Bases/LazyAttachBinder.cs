using System;

namespace DevKit.UI.MVVM.Bases
{
    public class LazyAttachBinder<TView, TViewModel> : AttachBinder<TView, TViewModel>
        where TView : ScreenToolkitAttach<TViewModel>
        where TViewModel : ScreenViewModel
    {
        private readonly Func<TViewModel> _viewModelFactory;

        public LazyAttachBinder(Func<TViewModel> viewModelFactory, IRootUIBinder rootUIBinder, TView view) : base(
            rootUIBinder, view)
        {
            _viewModelFactory = viewModelFactory;
        }

        public override TViewModel GetViewModel() => _viewModelFactory();
    }
}