using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public abstract class AttachableToolkitView : ToolkitView, IAttachableToolkitView
    {
        public virtual int ToolkitSortOrder { get; }

        VisualElement IAttachableToolkitView.Root => Root;
        GameObject IAttachableToolkitView.GameObject => gameObject;

        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
    
    public abstract class AttachableToolkitView<T> : ToolkitView<T>, IAttachableToolkitView where T : IViewModel
    {
        public virtual int ToolkitSortOrder { get; }
        
        VisualElement IAttachableToolkitView.Root => Root;
        GameObject IAttachableToolkitView.GameObject => gameObject;

        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
}