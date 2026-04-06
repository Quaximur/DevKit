using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// <para>Binder for View and ViewModel, assuming a Singleton ViewModel (created only once). 
    /// Specifies the TViewModel with which the View works.</para>
    /// 
    /// To work with a concrete ViewModel
    /// rather than an abstract one (if specified in the View, e.g., IView&lt;TViewModel&gt;), 
    /// use the class with extended generic parameters 
    /// <see cref="PersistentBinder{TView, TViewModel, AViewModel}"/>.
    /// </summary>
    public class PersistentBinder<TView, TViewModel> : PersistentBinder<TView, TViewModel, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel, IDisposable
    {
        public PersistentBinder(TView view, TViewModel viewModel) : base(view, viewModel)
        {
        }
    }

    /// <summary>
    /// <para>Binder for View and ViewModel, assuming a Singleton ViewModel (created only once).</para>
    /// 
    /// Here the View may have a dependency on an abstract AViewModel (e.g., IView&lt;AViewModel&gt;),
    /// while actually being linked to a concrete TViewModel that implements/inherits from
    /// AViewModel (where AViewModel : IViewModel).
    /// </summary>
    public class PersistentBinder<TView, TViewModel, AViewModel> : BaseBinder<TView, TViewModel, AViewModel>
        where TView : IView<AViewModel>
        where AViewModel : IViewModel
        where TViewModel : AViewModel, IDisposable
    {
        protected TViewModel _persistentViewModel;

        public PersistentBinder(TView view, TViewModel viewModel)
        {
            _view = view;
            _persistentViewModel = viewModel;
        }

        public override AViewModel Open()
        {
            _view.Bind(_persistentViewModel);
            _view.Show();
            
            return _persistentViewModel;
        }

        public override void Close()
        {
            _view.Hide();
            _view.Dispose();
        }
    }
}