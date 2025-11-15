using System.Collections.Generic;

namespace DevKit.Utils
{
    public static class EnumerableExtensions
    {
        public static InspectorReadonlyList<T> ToInspectorReadonlyList<T>(this IEnumerable<T> values) =>
            new InspectorReadonlyList<T>(values);
    }
}