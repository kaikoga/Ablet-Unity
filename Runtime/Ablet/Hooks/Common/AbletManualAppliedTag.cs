using Ablet.API.V1.Attributes;
using UnityEngine;

namespace Ablet.Hooks.Common
{
    [AddComponentMenu("")]
    public class AbletManualAppliedTag : MonoBehaviour, IAbletManualAppliedTag, IAbletInteropEditorOnly
    {
        public string platformId;
        public string subplatformId;
        
        [AbletInitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            AbletHooksUtil.AddManualAppliedTagInjected += AddManualAppliedTag;
        }
        
        static void AddManualAppliedTag(GameObject rootObject, string platformId, string subplatformId)
        {
            var marker = rootObject.AddComponent<AbletManualAppliedTag>();
            marker.platformId = platformId;
            marker.subplatformId = subplatformId;
        }
    }
}
