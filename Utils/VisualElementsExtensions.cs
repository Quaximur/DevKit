using UnityEngine.UIElements;

namespace DevKit.Utils
{
    public static class VisualElementsExtensions
    {
        public static VisualElement GetFirstOfTypeWithUserData<T>(this VisualElement ve)
        {
            while (ve != null)
            {
                if (ve.userData is T) return ve;
                ve = ve.parent;
            }
            return null;
        }
    }
}