using System;

namespace DevKit.UI.MVVM
{
    public interface IAttachableView : IView, IDisposable
    {
        /// <summary>
        /// Views with higher priority are placed on top of views with lower priority,
        /// making them visually closer to the user.
        /// </summary>
        public int Priority { get; }

        public void Attach(IRootUI rootUI);
        public void Detach(IRootUI rootUI);
    }
}