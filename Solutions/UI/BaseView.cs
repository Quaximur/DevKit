using System;
using UnityEngine;

namespace DevKit.UI
{
    /// <summary>
    /// Base implementation of a View through MonoBehaviour.
    /// Further implementation via Canvas or UI Toolkit can be built upon this class (through inheritance).
    /// </summary>
    public abstract class BaseView : MonoBehaviour, IView, IDisposableNotifier 
    {
        public event Action OnDisposed;

        public virtual void Init()
        {
            
        }
        
        public abstract void Show();
        
        public abstract void Hide();

        public virtual void Dispose()
        {
            OnDisposed?.Invoke();
        }
    }
}