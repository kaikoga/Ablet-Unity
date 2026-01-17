using Ablet.Repositories;
using UnityEditor;

namespace Ablet.Ndmf
{
    public static class AbletNdmfStaticMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
        }

        [MenuItem("Tools/Ablet/Ablet on NDMF", false, 41)]
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

        [MenuItem("Tools/Ablet/Ablet Prefer NDMF/Enable Ablet Prefer NDMF", false, 40)]
        static void AbletPreferNdmf()
        {
            EditorSettingsRepository.Instance.Value.AbletPreferNdmf = !EditorSettingsRepository.Instance.Value.AbletPreferNdmf;
            EditorSettingsRepository.Instance.Save();
            SetChecked();
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Ablet Prefer NDMF/Enable Ablet Prefer NDMF", EditorSettingsRepository.Instance.Value.AbletPreferNdmf);
            Menu.SetChecked("Tools/Ablet/Ablet on NDMF", EditorSettingsRepository.Instance.Value.IsAbletOnNdmf);
        }
    }
}
