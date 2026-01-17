#if !ABLET_PREFER_NDMF

using Ablet.Repositories;
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

        [MenuItem("Tools/Ablet/NDMF on Ablet", true, 42)]
        static bool ValidateNdmfOnAblet()
        {
            return NdmfConfigAccess.IsNdmfOnAbletAvailable();
        }

        [MenuItem("Tools/Ablet/NDMF on Ablet", false, 42)]
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

        [MenuItem("Tools/Ablet/Prefer Ablet", false, 43)]
        static void PreferAblet()
        {
            EditorSettingsRepository.Instance.Value.PreferAblet = !EditorSettingsRepository.Instance.Value.PreferAblet;
            EditorSettingsRepository.Instance.Save();
            SetChecked();
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/NDMF on Ablet", EditorSettingsRepository.Instance.Value.IsNdmfOnAblet);
            Menu.SetChecked("Tools/Ablet/Prefer Ablet", EditorSettingsRepository.Instance.Value.PreferAblet);
        }
    }
}

#endif
