#if ABLET_LOCH

using Silksprite.Loch.Core;
using UnityEditor;

namespace Ablet.Loch
{
    static class AbletLochMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
            LochRepository.Instance.OnLanguageChanged += SetChecked;
        }

        [MenuItem("Tools/Ablet/Sync Translation (Loch) with NDMF", false, 44)]
        static void SyncWithNdmf()
        {
            NdmfSyncSettingRepository.Instance.IsNdmfSyncEnabled = !NdmfSyncSettingRepository.Instance.IsNdmfSyncEnabled;
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Sync Translation (Loch) with NDMF", NdmfSyncSettingRepository.Instance.IsNdmfSyncEnabled);
        }
    }
}

#endif