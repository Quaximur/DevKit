using UnityEngine.UIElements;

namespace DevKit.Solutions.UI
{
    /// <summary>
    /// Шаблон для представлений, реализованных через UI Toolkit.
    /// </summary>
    public interface IToolkitView : IView
    {
        public VisualElement InitAndGetRoot();
    }
}