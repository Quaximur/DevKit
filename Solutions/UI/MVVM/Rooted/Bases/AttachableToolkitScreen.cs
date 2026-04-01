using UnityEngine;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableToolkitScreen<T> : AttachableToolkitView<T>, IScreenAttach<T> 
        where T : IScreenViewModel
    {
        public override int ToolkitSortOrder => _priority;

        [SerializeField]
        [Tooltip("Views with higher priority are placed on top of views with lower priority, " +
                 "making them visually closer to the user.")]
        protected int _priority;
        
        protected IOpenStateController _openStateController;

        protected override void OnBind(T viewModel)
        {
            _openStateController ??= GetOpenStateController();
            _openStateController.Bind(this, viewModel);
        }

        protected virtual void OnOpenStateChanged(bool isOpened)
        {
            _openStateController.OnOpenStateChanged(isOpened);
        }

        protected virtual void OnOpening()
        {
            _openStateController.OnOpening();
        }

        protected virtual void OnClosing()
        {
            _openStateController.OnClosing();
        }

        protected virtual IOpenStateController GetOpenStateController()
        {
            return new BaseOpenStateController();
        }

        public override void Dispose()
        {
            _openStateController.Dispose();

            base.Dispose();
        }
    }
}