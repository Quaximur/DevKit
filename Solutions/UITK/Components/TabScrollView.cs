using UnityEngine.UIElements;

namespace DevKit.UITK
{
    [UxmlElement]
    public partial class TabScrollView : Tab
    {
        public override VisualElement contentContainer => ScrollView.contentContainer;

        [UxmlAttribute]
        public ScrollViewMode ScrollViewMode
        {
            get => ScrollView.mode;
            set => ScrollView.mode = value;
        }

        [UxmlAttribute]
        public ScrollerVisibility HorizontalScrollerVisibility
        {
            get => ScrollView.horizontalScrollerVisibility;
            set => ScrollView.horizontalScrollerVisibility = value;
        }

        [UxmlAttribute]
        public ScrollerVisibility VerticalScrollerVisibility
        {
            get => ScrollView.verticalScrollerVisibility;
            set => ScrollView.verticalScrollerVisibility = value;
        }

        public readonly ScrollView ScrollView;

        public TabScrollView() : base()
        {
            ScrollView = new ScrollView()
            {
                horizontalScrollerVisibility = ScrollerVisibility.Hidden,
            };

            var tabContentContainer = this.Q(name: contentUssClassName);
            tabContentContainer.Add(ScrollView);
        }

        public void ScrollToTop()
        {
            ScrollView.verticalScroller.value = 0;
        }
    }
}