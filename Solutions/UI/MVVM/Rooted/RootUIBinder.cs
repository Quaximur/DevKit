using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public class RootUIBinder : MonoBehaviour, IRootUIBinder, IRootUI
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private Canvas _firstCanvas;
        [SerializeField] private RectTransform _canvasFirstUiRoot;
        [SerializeField] private RectTransform _canvasLastUiRoot;
        [SerializeField] private string _visualElementRootName;

        private VisualElement _visualElementRoot;
        private readonly HashSet<IAttachableView> _bindedViews = new();

#region IRootUIBinder
        public void SetView(IAttachableView view)
        {
            ClearViews();
            AddView(view);
        }

        public void SetViews(IEnumerable<IAttachableView> views)
        {
            ClearViews();
            AddViews(views);
        }

        public void SetViews(params IAttachableView[] views)
        {
            ClearViews();
            AddViews(views);
        }

        public void AddView(IAttachableView view)
        {
            view.Attach(this);
            _bindedViews.Add(view);
        }

        public void AddViews(params IAttachableView[] views)
        {
            foreach (var view in views)
                AddView(view);
        }

        public void AddViews(IEnumerable<IAttachableView> views)
        {
            foreach (var view in views)
                AddView(view);
        }

        public void ClearView(IAttachableView view)
        {
            view?.Detach(this);
            _bindedViews.Remove(view);
        }

        public void ClearViews()
        {
            foreach (var view in _bindedViews.ToArray())
                ClearView(view);
        }
#endregion

#region MonoBehaviour
        private void Awake()
        {
            _visualElementRoot = _document.rootVisualElement.Q(name: _visualElementRootName) ??
                                 _document.rootVisualElement;
            // _firstCanvas.gameObject.SetActive(false);
        }
#endregion

#region IRootUI
        /// <summary>
        /// This is only used by UI Toolkit Views in terms of implementation Visitor pattern. 
        /// For the scene UI binding use SetViews or AddViews method instead.
        /// </summary>
        public void Attach(VisualElement visualElement)
        {
            _visualElementRoot.Add(visualElement);
        }

        /// <summary>
        /// This is used by Canvas Views in terms of implementation Visitor pattern. 
        /// Can also be used with UI Toolkit View gameobjects just to hold them.
        /// For the scene UI binding use SetViews or AddViews method instead.
        /// </summary>
        public void Attach(GameObject gameObjectUI, int sortOrder)
        {
            switch (sortOrder)
            {
                case 0:
                    EnableFirstCanvas();
                    gameObjectUI.transform.SetParent(_canvasFirstUiRoot, false);
                    break;
                case 1:
                    gameObjectUI.transform.SetParent(_canvasLastUiRoot, false); break;
            }
        }

        public void Detach(VisualElement visualElement)
        {
            visualElement.RemoveFromHierarchy();
        }

        public void Detach(GameObject gameObjectUI)
        {
            Destroy(gameObjectUI);
            DisableFirstCanvasIfNeeded();
        }
#endregion

        private void EnableFirstCanvas()
        {
            _firstCanvas.gameObject.SetActive(true);
        }

        private void DisableFirstCanvasIfNeeded()
        {
            if (_canvasFirstUiRoot.childCount == 0)
                _firstCanvas.gameObject.SetActive(false);
        }
    }
}