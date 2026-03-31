using System;

namespace DevKit.UI.MVVM.Bases
{
    public interface IScreenViewModel : IViewModel, IDisposable
    {
        public event Action<bool> OnOpenStateChanged;
        public event Action OnClosingCompleted;

        public void Open();
        public void StartClosing();
        public void CompleteClosing();
    }
}