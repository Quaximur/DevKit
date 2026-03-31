using System;
using UnityEngine;

namespace DevKit.UI
{
    /// <summary>
    /// Базовая реализация представления через MonoBehaviour.
    /// От этого класса может отталкиваться (через наследование) дальнейшая реализация через Canvas или UI Toolkit.
    /// </summary>
    public abstract class BaseView : MonoBehaviour, IView, IDisposable
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