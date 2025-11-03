using System;

namespace DevKit.UI.MVVM
{
    public interface IAttachableView : IDisposable
    {
        public void Attach(IRootUI rootUI);
        public void Detach(IRootUI rootUI);
    }
}