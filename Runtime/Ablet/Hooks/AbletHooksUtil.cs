using UnityEngine;

namespace Ablet.Hooks
{
    public static class AbletHooksUtil
    {
        public delegate void AddManualAppliedTagHandler(GameObject gameObject, string platformId, string subplatformId);
        internal static event AddManualAppliedTagHandler? AddManualAppliedTagInjected;

        public static void AddManualAppliedTag(GameObject gameObject, string platformId, string subplatformId) => AddManualAppliedTagInjected?.Invoke(gameObject, platformId, subplatformId);
    }
}
