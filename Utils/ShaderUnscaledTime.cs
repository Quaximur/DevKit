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