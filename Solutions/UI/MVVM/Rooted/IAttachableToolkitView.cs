using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IAttachableToolkitView : IToolkitView, IAttachableView
    {
        /// <summary>
        /// Views with higher sort order are placed on top of views with lower priority,
        /// making them visually closer to the user.
        /// </summary>
        public int ToolkitSortOrder { get; }

        protected VisualElement Root { get; }
        protected GameObject GameObject { get; }
        
        void IAttachableView.Attach(IRootUI rootUI)
        {
            rootUI.Attach(GameObject);
            rootUI.Attach(Root, ToolkitSortOrder);
        }

        void IAttachableView.Detach(IRootUI rootUI)
        {
            Dispose();
            rootUI.Detach(Root);
            rootUI.Detach(GameObject);
        }
    }
}