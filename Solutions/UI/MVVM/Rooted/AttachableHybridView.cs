using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public abstract class AttachableHybridView : ToolkitView, IAttachableHybridView
    {
        public virtual int ToolkitSortOrder { get; }
        public virtual int CanvasSortOrder { get; }

        VisualElement IAttachableToolkitView.Root => Root;
        GameObject IAttachableToolkitView.GameObject => gameObject;
        GameObject IAttachableHybridView.GameObject => gameObject;
        GameObject IAttachableCanvasView.GameObject => gameObject;

        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
    
    public abstract class AttachableHybridView<T> : ToolkitView<T>, IAttachableHybridView 
        where T : IViewModel
    {
        public virtual int ToolkitSortOrder { get; }
        public virtual int CanvasSortOrder { get; }

        VisualElement IAttachableToolkitView.Root => Root;
        GameObject IAttachableToolkitView.GameObject => gameObject;
        GameObject IAttachableHybridView.GameObject => gameObject;
        GameObject IAttachableCanvasView.GameObject => gameObject;

        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
}