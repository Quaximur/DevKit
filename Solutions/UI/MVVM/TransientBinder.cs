using System;
using DevKit.Utils;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// <para>Binder for View and ViewModel, assuming a Transient ViewModel (created each time it is opened).
    /// Specifies the TViewModel with which the View works.</para> 
    /// 
    /// To work with a concrete ViewModel
    /// rather than an abstract one (if specified in the View, e.g., IView&lt;TViewModel&gt;), 
    /// use the class with extended generic parameters (<see cref="TransientBinder{TView, TViewModel, AViewModel}"/>).
    /// </summary>
    public abstract class TransientBinder<TView, TViewModel> : TransientBinder<TView, TViewModel, TViewModel>
        where TView : IView<TViewModel>
        where TViewModel : IViewModel, IDisposable
    {
        protected TransientBinder(TView view) : base(view)
        {
        }
    }

    /// <summary>
    /// <para>Binder for View and ViewModel, assuming a Transient ViewModel (created each time it is opened).</para>
    /// 
    /// Here the View may have a dependency on an abstract AViewModel (e.g., IView&lt;AViewModel&gt;),
    /// while actually being linked to a concrete TViewModel that implements/inherits from
    /// AViewModel (where AViewModel : IViewModel).
    /// </summary>
    public abstract class TransientBinder<TView, TViewModel, AViewModel> : BaseBinder<TView, TViewModel, AViewModel>
        where TView : IView<AViewModel>
        where AViewModel : IViewModel
        where TViewModel : AViewModel, IDisposable
    {
        public TransientBinder(TView view)
        {
            _view = view;
        }

        protected abstract TViewModel GetViewModel();

        public override AViewModel Open()
        {
            _currentViewModel = GetViewModel();
            _view.Bind(_currentViewModel);
            _view.Show();
            
            return _currentViewModel;
        }

        public override void Close()
        {
            _view.Hide();
            Disposes.ClearDispose(ref _currentViewModel);
            _view.Dispose();
        }
    }
}