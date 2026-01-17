using System.Collections.Generic;
using System.Linq;
using Ablet.InternalAPI.V1;
using Ablet.InternalAPI.V1.Attributes;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;

namespace Ablet.Registries
{
    class ApplyOnPlayRegistry : InstanceRegistryBase<IAbletApplyOnPlay>
    {
        public static readonly ApplyOnPlayRegistry Instance = new ApplyOnPlayRegistry();

        ApplyOnPlayRegistry()
        {
            Collect(new InstanceCollector<IAbletApplyOnPlay, AbletApplyOnPlayAttribute>());
        }

        public override IEnumerable<IAbletApplyOnPlay> All() => Unordered();

        public IAbletApplyOnPlay CurrentImpl
            => All()
                .Where(def => def.Available)
                .OrderBy(def => def.Priority).First();
    }
}
