using System.Collections.Generic;
using System.Linq;
using DevKit.Utils;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace DevKit.UI.MVVM
{
    public class RootUIBinder : MonoBehaviour, IRootUIBinder, IRootUI
    {
        [SerializeField] private UIDocument _documentPrefab;
        [SerializeField] private Canvas _canvasPrefab;
        [SerializeField] private Transform _layersRoot;

        private readonly HashSet<IAttachableView> _bindedViews = new();
        private ObjectPool<UIDocument> _documentPool;
        private ObjectPool<Canvas> _canvasPool;
        private readonly Dictionary<float, Canvas> _canvasLayersMap = new();
        private readonly Dictionary<float, UIDocument> _documentLayersMap = new();
        private readonly Dictionary<GameObject, Canvas> _canvasViewParentMap = new();
        private readonly Dictionary<VisualElement, UIDocument> _documentViewParentMap = new();

        #region MonoBehaviour
        private void Awake()
        {
            _documentPool = new ObjectPool<UIDocument>(
                createFunc: () =>
                {
                    var document = Instantiate(_documentPrefab);
                    document.transform.SetParent(_layersRoot, false);
                    return document;
                },
                actionOnGet: x => x.gameObject.SetActive(true),
                actionOnRelease: x => x.gameObject.SetActive(false),
                actionOnDestroy: x => Destroy(x.gameObject),
                defaultCapacity: 1,
                maxSize: 10);

            _canvasPool = new ObjectPool<Canvas>(
                createFunc: () =>
                {
                    var canvas = Instantiate(_canvasPrefab);
                    canvas.transform.SetParent(_layersRoot, false);
                    return canvas;
                },
                actionOnGet: x => x.gameObject.SetActive(true),
                actionOnRelease: x => x.gameObject.SetActive(false),
                actionOnDestroy: x => Destroy(x.gameObject),
                defaultCapacity: 1,
                maxSize: 10);
        }

        private void OnDestroy()
        {
            _documentPool?.Clear();
            _canvasPool?.Clear();
        }
        #endregion

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

        #region IRootUI
        /// <summary>
        /// This is only used by UI Toolkit Views in terms of implementation Visitor pattern. 
        /// For the scene UI binding use SetViews or AddViews method instead.
        /// </summary>
        void IRootUI.Attach(VisualElement visualElement, int sortOrder)
        {
            if (!_documentLayersMap.TryGetValue(sortOrder, out var document))
            {
                document = _documentPool.Get();
                document.panelSettings.sortingOrder = sortOrder;

                _documentLayersMap.Add(sortOrder, document);
            }

            if (!_documentViewParentMap.TryAdd(visualElement, document))
            {
                FLogger.LogWarning($"Reattaching VisualElement '{visualElement.name}'");
                _documentViewParentMap[visualElement] = document;
            }

            document.rootVisualElement.Add(visualElement);
        }

        /// <summary>
        /// This is used by Canvas Views in terms of implementation Visitor pattern. 
        /// Can also be used with UI Toolkit View gameobjects just to hold them.
        /// For the scene UI binding use SetViews or AddViews method instead.
        /// </summary>
        void IRootUI.Attach(GameObject gameObjectUI, int sortOrder)
        {
            if (!_canvasLayersMap.TryGetValue(sortOrder, out var canvas))
            {
                canvas = _canvasPool.Get();
                canvas.sortingOrder = sortOrder;

                _canvasLayersMap.Add(sortOrder, canvas);
            }

            if (!_canvasViewParentMap.TryAdd(gameObjectUI, canvas))
            {
                FLogger.LogWarning($"Reattaching GameObject '{gameObjectUI.name}'");
                _canvasViewParentMap[gameObjectUI] = canvas;
            }

            gameObjectUI.transform.SetParent(canvas.transform, false);
        }

        void IRootUI.Detach(VisualElement visualElement)
        {
            visualElement.RemoveFromHierarchy();

            if (_documentViewParentMap.TryGetValue(visualElement, out var document) &&
                document.rootVisualElement.childCount == 0)
            {
                _documentLayersMap.Remove(document.panelSettings.sortingOrder);
                _documentViewParentMap.Remove(visualElement);

                _documentPool.Release(document);
            }
        }

        void IRootUI.Detach(GameObject gameObjectUI)
        {
            if (_canvasViewParentMap.TryGetValue(gameObjectUI, out var canvas) &&
                canvas.transform.childCount == 1)
            {
                _canvasLayersMap.Remove(canvas.sortingOrder);
                _canvasViewParentMap.Remove(gameObjectUI);

                _canvasPool.Release(canvas);
            }

            Destroy(gameObjectUI);
        }
        #endregion
    }
}