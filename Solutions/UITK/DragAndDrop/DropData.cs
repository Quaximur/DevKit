using UnityEngine;

namespace DevKit.UITK
{
    public struct DropData<T>
    {
        public bool IsLeftInPlace;
        public T Dragged;
        public T Target;
        public Vector2 PointerPosition;
        public DropPosition DropPosition { get; set; }
    }

    public enum DropPosition
    {
        Before,
        After,
        Inside
    }
}