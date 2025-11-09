using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.API.Attributes;
using UnityEngine;

namespace Ablet.Repositories
{
    public class PlatformRepository : DefinitionRepositoryBase<IAbletPlatform, AbletPlatformAttribute>
    {
        public static readonly PlatformRepository Instance = new PlatformRepository();

        public override IEnumerable<IAbletPlatform> All() => Ordered();

        public Type[] EntrypointComponentTypes => Ordered().Select(platform => platform.EntryPointComponentType).ToArray();

        public IAbletPlatform GuessPlatform(GameObject entrypointObject)
        {
            return Ordered()
                .FirstOrDefault(platform => entrypointObject.TryGetComponent(platform.EntryPointComponentType, out _));
        }
    }
}
