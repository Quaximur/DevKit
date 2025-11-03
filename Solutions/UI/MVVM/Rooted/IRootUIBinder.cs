using System.Collections.Generic;

namespace DevKit.UI.MVVM
{
    public interface IRootUIBinder
    {
        public void SetView(IAttachableView view);

        public void SetViews(IEnumerable<IAttachableView> views);

        public void SetViews(params IAttachableView[] views);

        public void AddView(IAttachableView view);

        public void AddViews(params IAttachableView[] views);

        public void AddViews(IEnumerable<IAttachableView> views);
        
        public void ClearView(IAttachableView view);

        public void ClearViews();
    }
}