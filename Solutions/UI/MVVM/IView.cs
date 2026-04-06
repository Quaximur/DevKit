using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Contract for implementing views that have a connection to some ViewModel.
    /// </summary>
    public interface IView<T> : IDisposable, IView where T : IViewModel
    {
        public void Bind(T viewModel);
    }
}