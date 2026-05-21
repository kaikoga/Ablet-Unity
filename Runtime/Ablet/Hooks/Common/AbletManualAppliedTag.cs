using Ablet.API.V1.Attributes;
using UnityEngine;

namespace Ablet.Hooks.Common
{
    [AddComponentMenu("")]
    public class AbletManualAppliedTag : MonoBehaviour, IAbletManualAppliedTag, IAbletInteropEditorOnly
    {
        [AbletInitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            AbletHooksUtil.AddManualAppliedTagInjected += AddManualAppliedTag;
        }
        
        static void AddManualAppliedTag(GameObject rootObject)
        {
            rootObject.AddComponent<AbletManualAppliedTag>();
        }
    }
}
