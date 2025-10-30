using UnityEngine.UIElements;

namespace DevKit.Solutions.UI
{
    /// <summary>
    /// Шаблон для реализации контейнера UI-элементов (UI Toolkit).
    /// Используется во View.
    /// </summary>
    public interface IContentContainer
    {
        public void AddContentElement(VisualElement element);
    }
}