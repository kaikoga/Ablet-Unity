using Ablet.Hooks;
using UnityEngine;

namespace Ablet.Builtin.NdmfEmu
{
    [AddComponentMenu("")]
    public class NdmfAbletContextHolder : MonoBehaviour, IAbletEditorOnly
    {
        public object? NdmfBuildContext;
    }
}
