using Ablet.ProjectSettings;
using Ablet.Repositories;
using nadena.dev.ndmf.config;
using UnityEditor;

namespace Ablet.Ndmf
{
    public static class AbletNdmfMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
        }

        [MenuItem("Tools/Ablet/Ablet on NDMF", false, 11)]
        static void AbletOnNdmf()
        {
            EditorSettingsRepository.Instance.Value.NdmfInteropMode = EditorSettingsRepository.Instance.Value.NdmfInteropMode switch
            {
                NdmfInteropMode.AbletOnNdmf => NdmfInteropMode.None,
                _ => NdmfInteropMode.AbletOnNdmf
            };
            EditorSettingsRepository.Instance.Save();
            SetChecked();
        }

        [MenuItem("Tools/Ablet/NDMF on Ablet", true, 12)]
        static bool ValidateNdmfOnAblet()
        {
            return !Config.ApplyOnPlay;
        }

        [MenuItem("Tools/Ablet/NDMF on Ablet", false, 12)]
        static void NdmfOnAblet()
        {
            EditorSettingsRepository.Instance.Value.NdmfInteropMode = EditorSettingsRepository.Instance.Value.NdmfInteropMode switch
            {
                NdmfInteropMode.NdmfOnAblet => NdmfInteropMode.None,
                _ => NdmfInteropMode.NdmfOnAblet
            };
            EditorSettingsRepository.Instance.Save();
            SetChecked();
        }

        [MenuItem("Tools/Ablet/Prefer Ablet", false, 13)]
        static void PreferAblet()
        {
            EditorSettingsRepository.Instance.Value.PreferAblet = !EditorSettingsRepository.Instance.Value.PreferAblet;
            EditorSettingsRepository.Instance.Save();
            SetChecked();
            AbletDefineSymbolsApplier.Apply();
        }
        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Ablet on NDMF", EditorSettingsRepository.Instance.Value.IsAbletOnNdmf);
            Menu.SetChecked("Tools/Ablet/NDMF on Ablet", EditorSettingsRepository.Instance.Value.IsNdmfOnAblet);
            Menu.SetChecked("Tools/Ablet/Prefer Ablet", EditorSettingsRepository.Instance.Value.PreferAblet);
        }
    }
}
