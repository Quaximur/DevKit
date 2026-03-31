using UnityEngine;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableCanvasScreen<T> : AttachableCanvasView<T>, IScreenAttach<T>
        where T : IScreenViewModel
    {
        public override int CanvasSortOrder => _priority;

        [SerializeField]
        [Tooltip("Views with higher priority are placed on top of views with lower priority, " +
                 "making them visually closer to the user.")]
        protected int _priority;

        protected bool _isOpened = false; // View should be "closed" by default

        protected override void OnBind(T viewModel)
        {
            ViewModel.OnOpenStateChanged += OnOpenStateChanged;
        }

        protected virtual void OnOpenStateChanged(bool isOpened)
        {
            if (_isOpened = isOpened)
                return;

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
            if (ViewModel != null)
                ViewModel.OnOpenStateChanged -= OnOpenStateChanged;
                
            base.Dispose();
        }
    }
}