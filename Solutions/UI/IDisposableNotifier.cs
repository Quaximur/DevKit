using System;

namespace DevKit.UI
{
    public interface IDisposableNotifier : IDisposable
    {
        public event Action OnDisposed;
    }
}