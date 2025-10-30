using UnityEngine;

namespace Flofin.Utils
{
    public static class ScreenUtil
    {
        public static bool IsValidScreenPosition(Vector2 screenPosition)
        {
            return !float.IsInfinity(screenPosition.x) && !float.IsInfinity(screenPosition.y) &&
                !float.IsNaN(screenPosition.x) && !float.IsNaN(screenPosition.y) &&
                screenPosition.x >= 0 && screenPosition.y >= 0 &&
                screenPosition.x <= Screen.width && screenPosition.y <= Screen.height;
        }
    }
}