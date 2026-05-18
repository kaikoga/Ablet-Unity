using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;
using UnityEngine;

namespace Ablet.Registries
{
    public class SubplatformRegistry : IdModelRegistryBase<IAbletSubplatform, AbletSubplatform>
    {
        public static readonly SubplatformRegistry Instance = new SubplatformRegistry();

        SubplatformRegistry()
        {
            Collect(new ModelCollector<IAbletSubplatform, AbletSubplatformAttribute, AbletSubplatform>(def => new AbletSubplatform(def)));
        }

        public override IEnumerable<AbletSubplatform> All() => Unordered();

        public override bool TryGetById(string id, [MaybeNullWhen(false)] out AbletSubplatform value)
        {
            if (base.TryGetById(id, out value))
            {
                return true;
            }
            if (PlatformRegistry.Instance.TryGetById(id, out var platform))
            {
                value = new AbletSubplatform(new DefaultSubplatform(platform));
                return true;
            }
            return false;
        }

        IEnumerable<AbletSubplatform> UnorderedForPlatform(AbletPlatform platform)
            => Unordered().Where(subplatform => subplatform.PlatformId == platform.Id && subplatform.IsAvailable);

        public IEnumerable<AbletSubplatform> ForPlatform(AbletPlatform platform)
            => UnorderedForPlatform(platform).OrderBy(subplatform => subplatform.Priority);

        public AbletSubplatform GuessSubplatform(GameObject entrypointObject, AbletPlatform platform)
        {
            var subplatform = UnorderedForPlatform(platform)
                .OrderByDescending(subplatform => subplatform.IsPreferredSubplatform(entrypointObject))
                .ThenBy(subplatform => subplatform.Priority)
                .FirstOrDefault();
            if (subplatform != null)
            {
                return subplatform;
            }
            TryGetById(platform.Id, out subplatform);
            return subplatform!;
        }

        class DefaultSubplatform : IAbletSubplatform
        {
            readonly AbletPlatform _platform;

            public DefaultSubplatform(AbletPlatform platform) => _platform = platform;

            public string Id => _platform.Id;
            public string DisplayName => _platform.DisplayName;
            public string PlatformId => _platform.Id;
            public int Priority => int.MaxValue;
            public bool IsAvailable => true;
            public bool IsPreferredSubplatform(GameObject entrypointObject) => true;
        }
    }
}
