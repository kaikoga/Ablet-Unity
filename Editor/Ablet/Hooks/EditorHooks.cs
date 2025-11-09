using System.Linq;
using Ablet.EditorAPI;
using Ablet.EditorAPI.Attributes;
using Ablet.Repositories;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;

namespace Ablet.Hooks
{
    public static class EditorHooks
    {
        static readonly IAbletApplyOnPlay ApplyOnPlayImpl = DefinitionCollector<IAbletApplyOnPlay, AbletApplyOnPlayAttribute>.Collect()
            .Values
            .Where(def => def.Available)
            .OrderBy(def => def.Priority).First();

        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (!EditorSettingsRepository.Instance.Value.ApplyOnPlay)
            {
                return;
            }
            var applyOnPlayImpl = ApplyOnPlayImpl;
            applyOnPlayImpl.OnPlayModeStateChanged(playModeStateChange);
        }

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad()
        {
            if (!EditorSettingsRepository.Instance.Value.ApplyOnPlay)
            {
                return;
            }
            var applyOnPlayImpl = ApplyOnPlayImpl;
            applyOnPlayImpl.OnRuntimeInitializeOnLoad();
        }
    }
}
