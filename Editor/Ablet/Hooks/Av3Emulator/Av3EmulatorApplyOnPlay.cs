using System.Linq;
using Ablet.API;
using Ablet.InternalAPI.V1;
using Ablet.InternalAPI.V1.Attributes;
using Ablet.Querying;
using Lyuma.Av3Emulator.Runtime;
using UnityEditor;

namespace Ablet.Hooks.Av3Emulator
{
    [AbletApplyOnPlay]
    class Av3EmulatorApplyOnPlay : IAbletApplyOnPlay
    {
        string IAbletDefinition.Id => BuiltinApplyOnPlayIds.Av3Emulator;
        string IAbletDefinition.DisplayName => "Apply on Play (Av3Emulator)";
        int IAbletApplyOnPlay.Priority => 10000;

        bool IAbletApplyOnPlay.Available => AQueryContext.Immediate.GetSceneComponents<LyumaAv3Emulator>().ResolveNow().Any();

        void IAbletApplyOnPlay.OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            // do nothing
        }

        void IAbletApplyOnPlay.OnRuntimeInitializeOnLoad()
        {
            // Configure Av3Emulator 
            foreach (var av3Emulator in AQueryContext.Immediate.GetSceneComponents<LyumaAv3Emulator>().ResolveNow())
            {
                if (av3Emulator.enabled && av3Emulator.gameObject.activeInHierarchy)
                {
                    av3Emulator.RunPreprocessAvatarHook = true;
                }
            }
            // Ablet Apply on Play for VRC avatars will be processed through AbletVRChatBuildPreprocessor
            
            // ... but Ablet has to apply Apply on Play for non-VRChat avatars... 
            ApplyOnPlayImpl.OnRuntimeInitializeOnLoad(false);
        }
    }
}
