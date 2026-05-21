using nadena.dev.ndmf;
using UnityEngine;

namespace Ablet.Builtin.NdmfEmu
{
    [AddComponentMenu("")]
    public class NdmfAbletContextHolder : MonoBehaviour
        // INDMFEditorOnly is equivalent of IAbletInteropEditorOnly (avoid ifdef here)
        // this requires reference to VRCSDKBase.dll; otherwise: error CS0012: The type 'IEditorOnly' is defined in an assembly that is not referenced. You must add a reference to assembly 'VRCSDKBase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'.
        , INDMFEditorOnly
    {
        public object? NdmfBuildContext;
    }
}
