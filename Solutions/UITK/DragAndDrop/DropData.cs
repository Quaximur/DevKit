using UnityEngine;

namespace DevKit.UITK
{
    public struct DropData<T>
    {
        public bool IsLeftInPlace { get; set; }
        public T Dragged { get; set; }
        public T Target { get; set; }
        public Vector2 PointerPosition { get; set; }
        public DropPosition DropPosition { get; set; }
    }
}