using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;
using UnityEngine;

namespace Ablet.Registries
{
    public class PlatformRegistry : IdModelRegistryBase<IAbletPlatform, AbletPlatform>
    {
        public static readonly PlatformRegistry Instance = new PlatformRegistry();

        PlatformRegistry()
        {
            Collect(new ModelCollector<IAbletPlatform, AbletPlatformAttribute, AbletPlatform>(def => new AbletPlatform(def)));
        }

        public AbletPlatform ByPlatformHandle(IAbletPlatformHandle platformHandle)
        {
            _ = TryGetById(platformHandle.Id, out var platform);
            return platform!;
        }

        IEnumerable<AbletPlatform> Ordered() => Unordered().OrderBy(value => value.Priority);

        public override IEnumerable<AbletPlatform> All() => Ordered();

        public AbletPlatform RequirePlatform(GameObject entrypointObject)
        {
            if (!TryGuessPlatform(entrypointObject, out var platform))
            {
                throw new NotSupportedException("Platform not supported");
            }
            return platform;
        }

        public bool TryGuessPlatform(GameObject entrypointObject, [MaybeNullWhen(false)] out AbletPlatform platform)
        {
            platform = Ordered().FirstOrDefault(platform =>
                entrypointObject.TryGetComponent(platform.EntrypointComponentType, out var component)
                && platform.FilterEntrypoint(component));
            return platform != null;
        }

        public bool TryGuessEntrypointObject(
            GameObject childObject,
            [MaybeNullWhen(false)] out GameObject entrypoint,
            [MaybeNullWhen(false)] out AbletPlatform platform)
        {
            if (!childObject)
            {
                entrypoint = null;
                platform = null;
                return false;
            }
            var candidate = childObject.transform;
            while (candidate)
            {
                if (TryGuessPlatform(candidate.gameObject, out platform))
                {
                    entrypoint = candidate.gameObject;
                    return true;
                }
                candidate = candidate.parent;
            }
            entrypoint = null;
            platform = null;
            return false;
        }
    }
}
