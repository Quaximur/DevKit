using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace DevKit.Utils
{
    /// <summary>
    /// Надстройка над логгером UnityEngine.Debug с кастомизацией текста.
    /// Используется для визуального разделения пользовательских сообщений 
    /// и сообщений из движка/сторонних библиотек.
    /// </summary>
    public static class FLogger
    {
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string text)
        {
            Debug.LogError(F.Error(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogError<TSource>(string text)
        {
            LogError(NamedLog<TSource>(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string text)
        {
            Debug.Log(text);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void Log<TSource>(string text)
        {
            Log(NamedLog<TSource>(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogGood(string text)
        {
            Debug.Log(F.Good(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogGood<TSource>(string text)
        {
            LogGood(NamedLog<TSource>(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(string text)
        {
            Debug.LogWarning(F.Warning(text));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning<TSource>(string text)
        {
            LogWarning(NamedLog<TSource>(text));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string NamedLog<TSource>(string text) => $"[{typeof(TSource).Name}]: {text}";
    }
}