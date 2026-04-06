using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Contains logic for creating/destroying UI screens (UI opener/closer)
    /// with functionality for binding View to ViewModel.
    /// </summary>
    /// <typeparam name="TView">View (UI)</typeparam>
    /// <typeparam name="TViewModel">Concrete ViewModel that inherits from (implements) AViewModel</typeparam>
    /// <typeparam name="AViewModel">Abstract ViewModel that the View depends on</typeparam>
    public abstract class BaseBinder<TView, TViewModel, AViewModel> : ILinkEntry, IViewBinder<AViewModel>
        where TView : IView<AViewModel>
        where AViewModel : IViewModel
        where TViewModel : AViewModel, IDisposable
    {
        protected TView _view;
        protected TViewModel _currentViewModel;

        public abstract AViewModel Open();
        public abstract void Close();

        void ILinkEntry.Open()
        {
            Open();
        }
    }
}