using UnityEngine.UIElements;

namespace DevKit.UI
{
    /// <summary>
    /// Contract for VisualElement container implementation.
    /// </summary>
    public interface IContentContainer
    {
        public void AddContentElement(VisualElement element);
    }
}