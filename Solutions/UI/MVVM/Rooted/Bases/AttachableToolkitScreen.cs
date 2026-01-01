using R3;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableToolkitScreen<T> : AttachableToolkitView<T>, IScreenAttach<T> where T : IScreenViewModel
    {
        protected CompositeDisposable _disposables;

        protected override void OnBind(T viewModel)
        {
            _disposables = new();
            
            ViewModel.OnOpenStateChanged
                .Skip(1)
                .Subscribe(OnOpenStateChanged)
                .AddTo(_disposables);
        }

        protected virtual void OnOpenStateChanged(bool isOpened)
        {
            if (isOpened)
                OnOpening();
            else
                OnClosing();
        }

        protected virtual void OnOpening()
        {
            Show();
        }

        protected virtual void OnClosing()
        {
            ViewModel.CompleteClosing();
        }

        public override void Dispose()
        {
            Disposes.ClearDispose(ref _disposables);
            base.Dispose();
        }
    }
}