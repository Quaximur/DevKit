using System;
using R3;

namespace DevKit.UI.MVVM.Bases
{
    public interface IScreenViewModel : IViewModel, IDisposable
    {
        public Observable<bool> OnOpenStateChanged { get; }
        public Observable<Unit> OnClosingCompleted { get; }

        public void Open();
        public void StartClosing();
        public void CompleteClosing();
    }
}