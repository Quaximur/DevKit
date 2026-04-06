using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Util ViewModel implementation with IDisposable interface
    /// </summary>
    public abstract class BaseViewModel : IViewModel, IDisposable
    {
        public virtual void Dispose()
        {
        }
    }
}