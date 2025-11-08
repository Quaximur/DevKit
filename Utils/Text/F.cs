using System.Runtime.CompilerServices;

namespace DevKit.Utils
{
    /// <summary>
    /// Форматирование строк (F - Format).
    /// </summary>
    public static class F
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Error(string text) => $"<color=#FFA500>{text}</color>";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Warning(string text) => $"<color=#FF8F5C>{text}</color>";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Good(string text) => $"<color=green>{text}</color>";
    }
}