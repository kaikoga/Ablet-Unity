using Ablet.Building;
using Ablet.InternalAPI.V1;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

namespace Ablet.Hooks
{
    public static class EditorHooks
    {
        static IAbletApplyOnPlay CurrentApplyOnPlayImpl => ApplyOnPlayRegistry.Instance.CurrentImpl;

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
            CurrentApplyOnPlayImpl.OnPlayModeStateChanged(playModeStateChange);
            if (playModeStateChange == PlayModeStateChange.ExitingPlayMode)
            {
                AssetPersister.ClearTempAssets();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad()
        {
            if (!EditorSettingsRepository.Instance.Value.ApplyOnPlay)
            {
                return;
            }
            CurrentApplyOnPlayImpl.OnRuntimeInitializeOnLoad();
        }
    }
}
