using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public interface IRootUI
    {
        public void Attach(VisualElement visualElement);
        public void Attach(GameObject gameObjectUI, int order = 0);

        public void Detach(VisualElement visualElement);
        public void Detach(GameObject gameObjectUI);
    }
}
