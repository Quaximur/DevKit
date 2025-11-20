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
        private readonly Func<T, VisualElement> _getLinedElement;

        public DragAndDropController(
            VisualElement root,
            VisualElement dropContainer,
            Func<T, T, bool> validation,
            Func<T, VisualElement> getLinedElement)
        {
            _root = root;
            _dropContainer = dropContainer;
            _validation = validation;
            _getLinedElement = getLinedElement;
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
            _dropIndicator = new VisualElement();
            _dropIndicator.AddToClassList("drop-indicator");
            _dropIndicator.style.display = DisplayStyle.None;

            // Стили для линии-индикатора
            _dropIndicator.style.position = Position.Absolute;
            _dropIndicator.style.height = 2f;
            _dropIndicator.style.backgroundColor = new Color(0f, 0.7f, 1f, 1f); // Синий цвет
            // _dropIndicator.style.zIndex = 9999;

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
                ClearDropIndicators();
                return;
            }

            var container = picked.GetFirstOfTypeWithUserData<T>();
            if (container == null)
            {
                SetTargetToNothing();
                return;
            }

            if (container.userData is not T targetItem)
                targetItem = default;

            if (!_validation(_draggedItem, targetItem))
            {
                SetTargetToNothing();
                return;
            }

            _dropTargetElement = container;
            var linedElement = _getLinedElement(targetItem);

            if (linedElement == null)
                ClearDropIndicators();
            else
                ShowElementIndicator(linedElement, mousePosition);

            void SetTargetToNothing()
            {
                _dropTargetElement = null;
            }
        }

        private void ShowElementIndicator(VisualElement targetElement, Vector2 mousePosition)
        {
            var worldBound = targetElement.worldBound;
            var centerY = worldBound.center.y;

            // Определяем позицию вставки (до или после элемента)
            if (mousePosition.y >= centerY)
            {
                _currentDropPosition = DropPosition.After;
                ShowLineIndicator(worldBound.x + 2f, worldBound.yMax - worldBound.height + 2f, worldBound.xMax - 2f);
            }
            if (mousePosition.y < centerY)
            {
                _currentDropPosition = DropPosition.Before;
                ShowLineIndicator(worldBound.x + 2f, worldBound.y - worldBound.height, worldBound.xMax - 2f);
            }
        }

        private void ShowLineIndicator(float x, float y, float xMax)
        {
            _dropIndicator.style.left = x;
            _dropIndicator.style.top = y;
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
