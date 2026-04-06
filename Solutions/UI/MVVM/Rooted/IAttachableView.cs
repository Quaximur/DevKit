using System;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// View that can be attached to some root UI.
    /// </summary>
    public interface IAttachableView : IView, IDisposable
    {
        public void Attach(IRootUI rootUI);
        public void Detach(IRootUI rootUI);
    }
}