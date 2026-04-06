using System.Collections.Generic;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Template for Root UI object that holds child Views.
    /// </summary>
    public interface IRootUIBinder
    {
        /// <summary>
        /// Clears the root UI and adds new View.
        /// </summary>
        public void SetView(IAttachableView view);

        /// <summary>
        /// Clears the root UI and adds new Views.
        /// </summary>
        public void SetViews(IEnumerable<IAttachableView> views);

        /// <summary>
        /// Clears the root UI and adds new Views.
        /// </summary>
        public void SetViews(params IAttachableView[] views);
        
        /// <summary>
        /// Adds new View to the existing ones.
        /// </summary>
        public void AddView(IAttachableView view);

        /// <summary>
        /// Adds new Views to the existing ones.
        /// </summary>
        public void AddViews(params IAttachableView[] views);

        /// <summary>
        /// Adds new Views to the existing ones.
        /// </summary>
        public void AddViews(IEnumerable<IAttachableView> views);
        
        public void ClearView(IAttachableView view);

        public void ClearViews();
    }
}