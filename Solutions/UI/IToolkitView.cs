using UnityEngine.UIElements;

namespace DevKit.UI
{
    /// <summary>
    /// Template for views implemented via UI Toolkit.
    /// </summary>
    public interface IToolkitView : IView
    {
        public VisualElement InitAndGetRoot();
    }
}