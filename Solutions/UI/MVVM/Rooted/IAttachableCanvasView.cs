using UnityEngine;

namespace DevKit.UI.MVVM
{
    public interface IAttachableCanvasView : IAttachableView
    {
        /// <summary>
        /// Views with higher priority are placed on top of views with lower priority,
        /// making them visually closer to the user.
        /// </summary>
        public int Priority { get; }
        protected GameObject GameObject { get; }

        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(GameObject, Priority);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(GameObject);
        }
    }
}