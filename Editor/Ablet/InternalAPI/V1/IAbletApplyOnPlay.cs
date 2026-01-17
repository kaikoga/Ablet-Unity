using Ablet.API;
using UnityEditor;

namespace Ablet.InternalAPI.V1
{
    public interface IAbletApplyOnPlay : IAbletDefinition
    {
        int Priority { get; }
        bool Available { get; }
        void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange);
        void OnRuntimeInitializeOnLoad();
    }
}
