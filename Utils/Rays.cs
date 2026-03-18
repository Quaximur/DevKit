using UnityEngine;

namespace DevKit.Utils
{
    public static class Rays
    {
        public static float GetMaxDistance(RaycastHit[] hits, int hitCount)
        {
            float maxDistance = 0f;
            for (var i = 0; i < hitCount; i++)
            {
                if (hits[i].distance > maxDistance)
                    maxDistance = hits[i].distance;
            }

            return maxDistance;
        }

        public static RaycastHit GetClosest(RaycastHit[] hits, int hitCount)
        {
            var closestHit = hits[0];
            if (hitCount > 1)
            {
                for (var i = 1; i < hitCount; i++)
                {
                    var hitI = hits[i];
                    if (hitI.distance < closestHit.distance)
                        closestHit = hitI;
                }
            }

            return closestHit;
        }
    }
}