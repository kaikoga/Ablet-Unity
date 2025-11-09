using System.Linq;
using Ablet.API;
using Ablet.EditorAPI;
using Ablet.EditorAPI.Attributes;
using Ablet.Querying;
using Lyuma.Av3Emulator.Runtime;
using UnityEditor;

namespace Ablet.Hooks.Av3Emulator
{
    [AbletApplyOnPlay]
    public class Av3EmulatorApplyOnPlay : IAbletApplyOnPlay
    {
        string IAbletDefinitionBase.Id => "ablet.hooks.apply-on-play.av3emulator";
        string IAbletDefinitionBase.DisplayName => "Apply on Play (Av3Emulator)";
        int IAbletDefinitionBase.Priority => 10000;

        bool IAbletApplyOnPlay.Available => AQuery.GetComponents<LyumaAv3Emulator>().Query().Any();

        void IAbletApplyOnPlay.OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            // do nothing
        }

        void IAbletApplyOnPlay.OnRuntimeInitializeOnLoad()
        {
            // Configure Av3Emulator 
            foreach (var av3Emulator in AQuery.GetComponents<LyumaAv3Emulator>().Query())
            {
                if (av3Emulator.enabled && av3Emulator.gameObject.activeInHierarchy)
                {
                    av3Emulator.RunPreprocessAvatarHook = true;
                }
            }
            // Ablet Apply on Play will be processed through AbletVRChatBuildPreprocessor
        }
    }
}
