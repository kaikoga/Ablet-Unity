using Ablet.API;
using UnityEditor;

namespace Ablet.EditorAPI
{
    public interface IAbletApplyOnPlay : IAbletDefinitionBase
    {
        bool Available { get; }
        void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange);
        void OnRuntimeInitializeOnLoad();
    }
}
