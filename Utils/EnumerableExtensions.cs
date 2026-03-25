using System;
using System.Collections.Generic;

namespace DevKit.Utils
{
    public static class EnumerableExtensions
    {
        public static InspectorReadonlyList<T> ToInspectorReadonlyList<T>(this IEnumerable<T> values)
        {
            return new InspectorReadonlyList<T>(values);
        }

        public static int IndexWhere<T>(this IEnumerable<T> values, Predicate<T> predicate)
        {
            var i = 0;
            foreach (var value in values)
            {
                if (predicate(value))
                    return i;

                i++;
            }

            return -1;
        }
        
        public static bool TryRemoveFirstWhere<T>(this IList<T> list, Predicate<T> predicate, out T removed)
        {
            var index = list.IndexWhere(predicate);

            if (index == -1)
            {
                removed = default;
                return false;
            }

            removed = list[index];
            list.RemoveAt(index);

            return true;
        }

        public static bool TryRemoveFirstWhere<T>(this IList<T> list, Predicate<T> predicate)
        {
            var index = list.IndexWhere(predicate);

            if (index == -1)
                return false;

            list.RemoveAt(index);

            return true;
        }

        public static int LastIndexWhere<T>(this IEnumerable<T> values, Predicate<T> predicate)
        {
            if (values is IReadOnlyList<T> list)
            {
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    if (predicate(list[i]))
                        return i;
                }

                return -1;
            }
            else
            {
                var currentIndex = 0;
                var lastIndex = -1;

                foreach (var value in values)
                {
                    if (predicate(value))
                        lastIndex = currentIndex;
                    currentIndex++;
                }

                return lastIndex;
            }
        }
    }
}
