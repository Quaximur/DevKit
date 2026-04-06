using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    /// <summary>
    /// Part of Visiter pattern that is used by Views to attach to root UI
    /// without knowing concrete View type (Canvas, UI Toolkit or Hybrid).
    /// </summary>
    public interface IRootUI
    {
        public void Attach(VisualElement visualElement, int sortOrder = 0);
        public void Attach(GameObject gameObjectUI, int sortOrder = 0);

        public void Detach(VisualElement visualElement);
        public void Detach(GameObject gameObjectUI);
    }
}
