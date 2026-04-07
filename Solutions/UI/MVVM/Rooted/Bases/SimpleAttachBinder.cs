using System;

namespace DevKit.UI.MVVM.Bases
{
    /// <summary>
    /// Most commonly used Binder for UI with IRootUIBinder workflow 
    /// with custom View and ViewModel factories.
    /// </summary>
    public class SimpleAttachBinder<TView, TViewModel> : AttachBinder<TView, TViewModel>
        where TView : IScreenAttach<TViewModel>, IDisposableNotifier
        where TViewModel : IScreenViewModel
    {
        private readonly Func<TViewModel> _viewModelFactory;
        private readonly Func<TView> _viewFactory;

        public SimpleAttachBinder(Func<TViewModel> viewModelFactory, IRootUIBinder rootUIBinder, 
            Func<TView> viewFactory) :
            base(rootUIBinder)
        {
            _viewModelFactory = viewModelFactory;
            _viewFactory = viewFactory;
        }

        protected override TViewModel GetViewModel() => _viewModelFactory();
        
        protected override TView GetView() => _viewFactory();
    }
}