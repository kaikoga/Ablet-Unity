#if !ABLET_PREFER_NDMF

using Ablet.Building;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEditor;

namespace Ablet
{
    static class AbletMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
            EditorSettingsRepository.Instance.OnChanged += SetChecked;
            EditorStateRepository.Instance.OnChanged += SetChecked;
        }

        [MenuItem("GameObject/Ablet/Manual Apply", true, 0)]
        [MenuItem("Tools/Ablet/Manual Apply", true, 0)]
        static bool ValidateManualApplyToGameObject()
        {
            return Selection.activeGameObject
                   && PlatformRegistry.Instance.TryGuessPlatform(Selection.activeGameObject, out _);
        }

        [MenuItem("GameObject/Ablet/Manual Apply", false, 0)]
        [MenuItem("Tools/Ablet/Manual Apply", false, 0)]
        static void ManualApplyToGameObject()
        {
            AbletFacade.ManualApplyToGameObject(Selection.activeGameObject);
        }

        [MenuItem("GameObject/Ablet/Flush Manual Apply Prefabs", false, 1)]
        [MenuItem("Tools/Ablet/Flush Manual Apply Prefabs", false, 1)]
        static void FlushManualApplyPrefabs()
        {
            AssetPersister.InteractiveClearManualAssets(true);
        }

        [MenuItem("Tools/Ablet/Apply on Play", false, 20)]
        static void ApplyOnPlay()
        {
            EditorSettingsRepository.Instance.Value.ApplyOnPlay = !EditorSettingsRepository.Instance.Value.ApplyOnPlay;
            EditorSettingsRepository.Instance.Save();
        }

        [MenuItem("Tools/Ablet/Apply on Platform Build", false, 21)]
        static void ApplyOnPlatformBuild()
        {
            EditorSettingsRepository.Instance.Value.ApplyOnPlatformBuild = !EditorSettingsRepository.Instance.Value.ApplyOnPlatformBuild;
            EditorSettingsRepository.Instance.Save();
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Apply on Play", EditorSettingsRepository.Instance.Value.ApplyOnPlay);
            Menu.SetChecked("Tools/Ablet/Apply on Platform Build", EditorSettingsRepository.Instance.Value.ApplyOnPlatformBuild);
        }
    }
}

#endif
