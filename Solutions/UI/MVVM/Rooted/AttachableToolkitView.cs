using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public abstract class AttachableToolkitView : ToolkitView, IAttachableToolkitView
    {
        VisualElement IAttachableToolkitView.Root => Root;

        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
    
    public abstract class AttachableToolkitView<T> : ToolkitView<T>, IAttachableToolkitView where T : IViewModel
    {
        VisualElement IAttachableToolkitView.Root => Root;
        
        protected override void ApplyRootStyle(VisualElement root)
        {
            UxmlUtil.StyleAttachable(root);
        }
    }
}