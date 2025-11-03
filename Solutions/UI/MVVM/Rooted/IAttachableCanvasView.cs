using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IAttachableCanvasView : IAttachableView
    {
        public GameObject gameObject { get; }
        public int Order { get; }

        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(gameObject, Order);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(gameObject);
        }
    }
}