using R3;
using UnityEngine;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableHybridScreen<T> : AttachableHybridView<T>, IScreenAttach<T> 
        where T : IScreenViewModel
    {
        public override int ToolkitSortOrder => _toolkitPriority;
        public override int CanvasSortOrder => _canvasPriority;

        [SerializeField]
        [Tooltip("Views with higher priority are placed on top of views with lower priority, " +
                 "making them visually closer to the user.")]
        protected int _toolkitPriority;

        [SerializeField]
        [Tooltip("Views with higher priority are placed on top of views with lower priority, " +
                 "making them visually closer to the user.")]
        protected int _canvasPriority;

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