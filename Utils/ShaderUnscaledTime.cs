using System;
using R3;
using UnityEngine;

namespace DevKit.Utils
{
    public static class ShaderUnscaledTime
    {
        private const string UnscaledTimePropertyName = "_UnscaledTime";
        private readonly static int UnscaledTimePropertyId = Shader.PropertyToID(UnscaledTimePropertyName);
        private static IDisposable _disposable;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ForceReload()
        {
            if (UnityEditor.EditorSettings.enterPlayModeOptionsEnabled &&
                    UnityEditor.EditorSettings.enterPlayModeOptions.HasFlag(UnityEditor.EnterPlayModeOptions.DisableDomainReload))
            {
                Off();
            }
        }
#endif

        public static void On()
        {
            _disposable = Observable.EveryUpdate().Subscribe(SetShaderProperty);
        }

        public static void Off()
        {
            _disposable?.Dispose();
        }

        private static void SetShaderProperty(Unit _)
        {
            Shader.SetGlobalFloat(UnscaledTimePropertyId, Time.unscaledTime);
        }
    }
}