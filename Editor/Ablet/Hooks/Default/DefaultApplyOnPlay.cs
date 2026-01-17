using Ablet.API;
using Ablet.InternalAPI.V1;
using Ablet.InternalAPI.V1.Attributes;
using UnityEditor;

namespace Ablet.Hooks.Default
{
    [AbletApplyOnPlay]
    class DefaultApplyOnPlay : IAbletApplyOnPlay
    {
        string IAbletDefinition.Id => BuiltinApplyOnPlayIds.Default;
        string IAbletDefinition.DisplayName => "Apply on Play";
        int IAbletApplyOnPlay.Priority => int.MaxValue;

        bool IAbletApplyOnPlay.Available => true;

        void IAbletApplyOnPlay.OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            // do nothing
        }

        void IAbletApplyOnPlay.OnRuntimeInitializeOnLoad()
        {
            ApplyOnPlayImpl.OnRuntimeInitializeOnLoad(true);
        }
    }
}
