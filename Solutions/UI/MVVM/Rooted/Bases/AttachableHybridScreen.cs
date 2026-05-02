using UnityEngine;

namespace DevKit.UI.MVVM.Bases
{
    public abstract class AttachableHybridScreen<T> : AttachableHybridView<T>, IScreenAttach<T>, IOpenableView
        where T : IScreenViewModel
    {
        public override int ToolkitSortOrder => _toolkitLayer;
        public override int CanvasSortOrder => _canvasLayer;

        [SerializeField]
        [Tooltip("Views with higher layer are placed on top of views with lower layer, " +
                 "making them visually closer to the user.")]
        protected int _toolkitLayer;

        [SerializeField]
        [Tooltip("Views with higher layer are placed on top of views with lower layer, " +
                 "making them visually closer to the user.")]
        protected int _canvasLayer;

        protected IOpenStateController _openStateController;

        protected override void OnBind(T viewModel)
        {
            _openStateController ??= GetOpenStateController();
            _openStateController.Bind(this, viewModel);
        }
        
        public virtual void OnOpening()
        {
            Show();
        }
        
        public virtual void OnClosing()
        {
            ViewModel.CompleteClosing();
        }

        protected virtual IOpenStateController GetOpenStateController()
        {
            return new OpenStateController();
        }

        public override void Dispose()
        {
            _openStateController.Dispose();

            base.Dispose();
        }
    }
}