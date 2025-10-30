using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Шаблон для реализации представлений, имеющих связь с некоторой ViewModel.
    /// </summary>
    public interface IView<T> : IDisposable, IView where T : IViewModel
    {
        public void Bind(T viewModel);
    }
}