using Ablet.Models;
using Ablet.Registries;
using UnityEngine;

namespace Ablet.Hooks.Common
{
    [AddComponentMenu("Ablet/Ablet Select Subplatform")]
    [HelpURL("https://docs.kaikoga.net/ablet/components/ablet_select_subplatform")]
    public class AbletSelectSubplatform : MonoBehaviour, IAbletSubplatformMarkerComponent
    {
        [SerializeField] internal string subplatformId = "";

        public AbletSubplatform? Subplatform => SubplatformRegistry.Instance.TryGetById(subplatformId, out var subplatform) ? subplatform : null;
    }
}
