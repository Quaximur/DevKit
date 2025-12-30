using System;
using System.Collections.Generic;

namespace DevKit.Utils
{
    public static class EnumerableExtensions
    {
        public static InspectorReadonlyList<T> ToInspectorReadonlyList<T>(this IEnumerable<T> values) =>
            new InspectorReadonlyList<T>(values);

        public static int IndexWhere<T>(this IEnumerable<T> values, Func<T, bool> predicate)
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

        public static int LastIndexWhere<T>(this IEnumerable<T> values, Func<T, bool> predicate)
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
