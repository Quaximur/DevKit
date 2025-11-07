#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor.Build;

namespace DevKit.Saves
{
    [InitializeOnLoad]
    internal class NewtonsoftInit
    {
        private const string INUI_NEWTONSOFT_JSON = nameof(INUI_NEWTONSOFT_JSON);

        static NewtonsoftInit()
        {
            EditorApplication.delayCall += ExecuteCheck;
        }

        private static void ExecuteCheck()
        {
            try
            {
                var hasNewtonsoft = AppDomain.CurrentDomain.GetAssemblies()
                    .Any(assembly => assembly.GetName().Name == "Newtonsoft.Json");

                UpdateDefineSymbols(hasNewtonsoft);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to check Newtonsoft.Json: {ex.Message}");
            }
        }

        private static void UpdateDefineSymbols(bool hasNewtonsoft)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildGroup);
            var currentSymbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            var symbols = currentSymbols.Split(';').ToList();

            var changed = false;

            if (hasNewtonsoft && !symbols.Contains(INUI_NEWTONSOFT_JSON))
            {
                symbols.Add(INUI_NEWTONSOFT_JSON);
                changed = true;
            }
            else if (!hasNewtonsoft && symbols.Contains(INUI_NEWTONSOFT_JSON))
            {
                symbols.Remove(INUI_NEWTONSOFT_JSON);
                changed = true;
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, string.Join(";", symbols));
                Debug.Log($"{INUI_NEWTONSOFT_JSON} symbol {(hasNewtonsoft ? "added" : "removed")}");
            }
        }
    }
}
#endif