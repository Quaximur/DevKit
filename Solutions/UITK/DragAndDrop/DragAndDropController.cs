using System;
using System.Collections.Generic;
using DevKit.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevKit.UITK
{
    public class DragAndDropController<T>
    {
        public event Action<DropData<T>> OnItemDropped;
        public event Action<T> OnItemDragStart;
        public event Action<T> OnGroupHoverStart;

        private readonly VisualElement _root;
        private readonly VisualElement _dropContainer;
        private readonly Func<T, T, bool> _validation; // <T dragged, T target, bool>

        private Label _dragGhost;
        private VisualElement _draggingElement;
        private VisualElement _dropTargetElement;
        private T _draggedItem;
        private bool _isDragging;
        private Vector2 _dragStartPosition;
        private VisualElement _dropIndicator;
        private DropPosition _currentDropPosition;
        private const float DRAG_THRESHOLD = 5f;
        private const float DROP_TARGET_SIDE_ZONE_RATIO = 0.25f;
        private readonly Func<T, VisualElement> _getLinedElement;
        private readonly Func<T, bool> _isGroupElement;
        private readonly Func<T, bool> _isExpanded;
        private readonly Func<T, bool> _hasChildren;

        // Для определения зон внутри элемента
        private enum DropZone
        {
            Top,
            Center,
            Bottom
        }

        public DragAndDropController(
            VisualElement root,
            VisualElement dropContainer,
            Func<T, T, bool> validation,
            Func<T, VisualElement> getLinedElement,
            Func<T, bool> isGroupElement,
            Func<T, bool> isExpanded,
            Func<T, bool> hasChildren)
        {
            _root = root;
            _dropContainer = dropContainer;
            _validation = validation;
            _getLinedElement = getLinedElement;
            _isGroupElement = isGroupElement;
            _isExpanded = isExpanded;
            _hasChildren = hasChildren;

            CreateDragGhost();
            CreateDropIndicator();
            RegisterEventHandlers();
        }

        public void SetGhostText(string text)
        {
            _dragGhost.text = text;
        }

        private void CreateDragGhost()
        {
            _dragGhost = new Label();
            _dragGhost.AddToClassList("drag-ghost");
            _dragGhost.style.display = DisplayStyle.None;

            _root.Add(_dragGhost);
        }

        private void CreateDropIndicator()
        {
            _dropIndicator = new VisualElement
            {
                pickingMode = PickingMode.Ignore
            };
            _dropIndicator.AddToClassList("drop-indicator");
            _dropIndicator.style.display = DisplayStyle.None;
            _dropIndicator.style.position = Position.Absolute;

            _root.Add(_dropIndicator);
        }

        private void RegisterEventHandlers()
        {
            _root.RegisterCallback<PointerMoveEvent>(evt => Drag(evt));
            _root.RegisterCallback<PointerUpEvent>(evt => EndDrag(evt));
            _root.RegisterCallback<PointerLeaveEvent>(evt =>
            {
                _dropTargetElement = null;
                CancelDrag(evt.position);
            });
        }

        public void RegisterDragElement(VisualElement element, T item)
        {
            element.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 0)
                    StartDrag(item, element, evt);
            });

            element.RegisterCallback<PointerLeaveEvent>(evt =>
            {
                if (!_isDragging && IsItemsEqual(_draggedItem, item))
                {
                    _draggedItem = default;
                    _draggingElement = null;
                }
            });
        }

        private void StartDrag(T item, VisualElement element, PointerDownEvent evt)
        {
            if (!IsItemsEqual(_draggedItem, default))
                return;

            _draggedItem = item;
            _dragStartPosition = evt.position;
            _draggingElement = element;
        }

        private void Drag(PointerMoveEvent evt)
        {
            if (!_isDragging && !IsItemsEqual(_draggedItem, default))
            {
                float distance = Vector2.Distance(_dragStartPosition, evt.position);
                if (distance >= DRAG_THRESHOLD)
                {
                    _isDragging = true;
                    _dragGhost.style.display = DisplayStyle.Flex;
                    OnItemDragStart?.Invoke(_draggedItem);
                    _draggingElement.AddToClassList("drag-source");
                }
                else
                {
                    return;
                }
            }

            if (!_isDragging)
                return;

            _dragGhost.style.left = evt.position.x + 5;
            _dragGhost.style.top = evt.position.y + 5;
            UpdateDropTarget(evt.position);
        }

        private void UpdateDropTarget(Vector2 mousePosition)
        {
            ClearDropIndicators();

            VisualElement picked = _root.panel.Pick(mousePosition);
            if (picked == null)
            {
                SetTargetToNothing();
                return;
            }
            else if (picked == _dropContainer)
            {
                _dropTargetElement = picked;
                return;
            }

            var container = picked.GetFirstOfTypeWithUserData<T>();
            if (container == null || container.userData is not T targetItem)
            {
                SetTargetToNothing();
                return;
            }

            if (!_validation(_draggedItem, targetItem))
            {
                SetTargetToNothing();
                return;
            }

            _dropTargetElement = container;
            var linedElement = _getLinedElement(targetItem);
            if (linedElement != null)
                ShowElementIndicator(linedElement, targetItem, mousePosition);

            if (ShouldStartGroupExpand(targetItem))
                OnGroupHoverStart?.Invoke(targetItem);
        }

        private void SetTargetToNothing()
        {
            _dropTargetElement = null;
            _currentDropPosition = DropPosition.After;
        }

        private bool ShouldStartGroupExpand(T targetItem)
        {
            if (_isGroupElement == null || _isExpanded == null || _hasChildren == null) return false;

            return _isGroupElement(targetItem) &&
                   !_isExpanded(targetItem) &&
                   _hasChildren(targetItem) &&
                   _currentDropPosition == DropPosition.Inside;
        }

        private DropZone GetDropZone(VisualElement element, Vector2 mousePosition, T targetItem)
        {
            var worldBound = element.worldBound;
            var elementHeight = worldBound.height;
            var relativeY = mousePosition.y - worldBound.y;

            // Определяем зоны (верхняя DROP_TARGET_SIDE_ZONE_RATIO, центральная 1 - 2 * DROP_TARGET_SIDE_ZONE_RATIO, 
            // нижняя DROP_TARGET_SIDE_ZONE_RATIO)
            var topZoneHeight = elementHeight * DROP_TARGET_SIDE_ZONE_RATIO;
            var bottomZoneHeight = elementHeight * DROP_TARGET_SIDE_ZONE_RATIO;

            var isGroup = _isGroupElement != null && _isGroupElement(targetItem);
            var isExpanded = isGroup && _isExpanded != null && _isExpanded(targetItem);
            var hasChildren = isGroup && _hasChildren(targetItem);

            if (!isGroup)
            {
                // Для обычных элементов и свернутых групп
                if (mousePosition.y < worldBound.center.y)
                    return DropZone.Top;
                else
                    return DropZone.Bottom;
            }
            else
            {
                if (!isExpanded || !hasChildren)
                {
                    if (relativeY < topZoneHeight)
                        return DropZone.Top;
                    else if (relativeY > elementHeight - bottomZoneHeight)
                        return DropZone.Bottom;
                    else
                        return DropZone.Center;
                }
                else
                {
                    if (relativeY < topZoneHeight)
                        return DropZone.Top;
                    else
                        return DropZone.Center; // Нижняя часть развернутой группы = поместить внутрь
                }
            }
        }

        private void ShowElementIndicator(VisualElement targetElement, T targetItem, Vector2 mousePosition)
        {
            var worldBound = targetElement.worldBound;
            var dropZone = GetDropZone(targetElement, mousePosition, targetItem);
            var lineX = worldBound.x + 2f;
            var lineWidth = worldBound.xMax - 2f;

            switch (dropZone)
            {
                case DropZone.Top:
                    _currentDropPosition = DropPosition.Before;
                    ShowLineIndicator(lineX, worldBound.y - worldBound.height, lineWidth);
                    break;
                case DropZone.Bottom:
                    _currentDropPosition = DropPosition.After;
                    ShowLineIndicator(lineX, worldBound.yMax - worldBound.height, lineWidth);
                    break;
                case DropZone.Center:
                    _currentDropPosition = DropPosition.Inside;
                    ClearDropIndicators();
                    break;
            }
        }

        private void ShowLineIndicator(float x, float y, float xMax)
        {
            _dropIndicator.style.translate = new Translate(x, y);
            _dropIndicator.transform.position = new Vector3(x, y, 0);
            _dropIndicator.style.width = xMax - x;
            _dropIndicator.style.display = DisplayStyle.Flex;
        }

        private void ClearDropIndicators()
        {
            _dropIndicator.style.display = DisplayStyle.None;
        }

        private void EndDrag(PointerUpEvent evt)
        {
            CancelDrag(evt.position);
        }

        private void CancelDrag(Vector3 pointerPos)
        {
            if (!_isDragging)
            {
                if (!IsItemsEqual(_draggedItem, default))
                {
                    _draggedItem = default;
                    _draggingElement = null;
                }
                return;
            }

            if (_dropTargetElement != null)
            {
                if (_dropTargetElement.userData == null || _dropTargetElement.userData is not T targetItem)
                    targetItem = default;

                OnItemDropped?.Invoke(new DropData<T>
                {
                    Dragged = _draggedItem,
                    Target = targetItem,
                    PointerPosition = pointerPos,
                    DropPosition = _currentDropPosition
                });
            }

            _draggingElement?.RemoveFromClassList("drag-source");
            _draggingElement = null;
            _draggedItem = default;
            _dropTargetElement = null;
            _isDragging = false;

            _dragGhost.style.display = DisplayStyle.None;
            ClearDropIndicators();
        }

        public bool IsItemsEqual(T item1, T item2)
        {
            return EqualityComparer<T>.Default.Equals(item1, item2);
        }

        public void Dispose()
        {
            _dragGhost?.RemoveFromHierarchy();
        }
    }
}
