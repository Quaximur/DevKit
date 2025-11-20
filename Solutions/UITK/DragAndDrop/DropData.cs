using UnityEngine;

namespace DevKit.UITK
{
    public struct DropData<T>
    {
        public T Dragged;
        public T Target;
        public Vector2 PointerPosition;
    }
}