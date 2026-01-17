using Ablet.API;
using Ablet.InternalAPI.V1;
using Ablet.InternalAPI.V1.Attributes;
using Ablet.Repositories;
using nadena.dev.ndmf.config;
using UnityEditor;

namespace Ablet.Hooks.NdmfPlugin
{
    [AbletApplyOnPlay]
    class NdmfApplyOnPlay : IAbletApplyOnPlay
    {
        string IAbletDefinition.Id => BuiltinApplyOnPlayIds.NDMF;
        string IAbletDefinition.DisplayName => "Apply on Play (NDMF)";
        int IAbletApplyOnPlay.Priority => -1000;

        bool IAbletApplyOnPlay.Available => Config.ApplyOnPlay;

        void IAbletApplyOnPlay.OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            switch (playModeStateChange)
            {
                case PlayModeStateChange.ExitingEditMode:
                    // Ndmf on Ablet is disabled when NDMF Apply on Play is enabled because we cannot intercept non-VRChat avatars being handled by NDMF
                    // (VRChat avatars may trigger AbletVRChatBuildPreprocessor, but we prefer to align the behavior)
                    if (EditorSettingsRepository.Instance.Value.IsNdmfOnAblet)
                    {
                        EditorSettingsRepository.Instance.Value.NdmfInteropMode = NdmfInteropMode.AbletOnNdmf;
                    }
                    break;
            }
        }

        public void OnRuntimeInitializeOnLoad()
        {
            // do nothing because a NDMF Apply on Play is triggered
        }
    }
}
