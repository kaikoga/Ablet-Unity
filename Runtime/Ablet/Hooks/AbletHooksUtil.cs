using UnityEngine;

namespace Ablet.Hooks
{
    public static class AbletHooksUtil
    {
        public delegate void AddManualAppliedTagHandler(GameObject gameObject);
        internal static event AddManualAppliedTagHandler? AddManualAppliedTagInjected;

        public static void AddManualAppliedTag(GameObject gameObject) => AddManualAppliedTagInjected?.Invoke(gameObject);
    }
}
