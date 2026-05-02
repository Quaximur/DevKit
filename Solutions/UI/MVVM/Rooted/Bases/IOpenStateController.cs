using System;

namespace DevKit.UI.MVVM.Bases
{
    public interface IOpenStateController : IDisposable
    {
        public void Bind(IOpenableView view, IScreenViewModel viewModel);

        public void OnOpenStateChanged(bool isOpened);

        public void OnOpening();

        public void OnClosing();
    }
}