using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IRootUI
    {
        public void Attach(VisualElement visualElement, int sortOrder = 0);
        public void Attach(GameObject gameObjectUI, int sortOrder = 0);

        public void Detach(VisualElement visualElement);
        public void Detach(GameObject gameObjectUI);
    }
}
