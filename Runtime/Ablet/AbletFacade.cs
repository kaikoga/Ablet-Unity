using System.Collections.Generic;
using System.Linq;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.API.V1.Querying;
using Ablet.Building;
using Ablet.Models;
using Ablet.Planning;
using Ablet.Querying;
using Ablet.Registries;
using JetBrains.Annotations;
using UnityEngine;
using AQueryContext = Ablet.Querying.AQueryContext;

namespace Ablet
{
    [PublicAPI]
    public static class AbletFacade
    {
        public static GameObject ManualApplyToGameObject(GameObject entrypointObject)
        {
            var platform = PlatformRegistry.Instance.RequirePlatform(entrypointObject);
            return ManualApplyToGameObject(entrypointObject, platform);
        }

        public static GameObject ManualApplyToGameObject<T>(GameObject entrypointObject)
        where T : IAbletPlatform
        {
            var platform = PlatformRegistry.Instance.Get<T>();
            return ManualApplyToGameObject(entrypointObject, platform);
        }

        public static GameObject ManualApplyToGameObject(GameObject entrypointObject, IAbletPlatformHandle platformHandle)
        {
            var platform = PlatformRegistry.Instance.ByPlatformHandle(platformHandle);
            return ManualApplyToGameObject(entrypointObject, platform);
        }

        static GameObject ManualApplyToGameObject(GameObject entrypointObject, AbletPlatform platform)
        {
            var arguments = new BuildArgumentBuilder(entrypointObject, platform).ForManualApply();
            return BuildWithArguments(arguments);
        }

        public static GameObject BuildWithArguments(BuildArgument arguments)
        {
            var plan = AvatarBuildPlanner.Plan(LayerRegistry.Instance.All());
            var process = BuildProcess.FromPlan(plan, arguments);
            return process.Build();
        }

        public static AQuery<(GameObject gameObject, AbletPlatform platform)> QuerySceneEntrypoints(bool includeInactive)
        {
            return AQueryContext.Immediate.GetSceneEntrypoints(PlatformRegistry.Instance.All(), includeInactive);
        }

        public static IEnumerable<(GameObject gameObject, AbletPlatform platform)> GetSceneEntrypoints(bool includeInactive)
        {
            return QuerySceneEntrypoints(includeInactive).ResolveNow();
        }

        public static AQuery<(GameObject gameObject, AbletPlatform platform)> QueryEntrypointFor(GameObject child)
        {
            return AQueryContext.Immediate.Return(child).GetEntrypointFor(PlatformRegistry.Instance.All());
        }

        public static bool TryGetEntrypointFor(GameObject child, out GameObject entrypointObject, out AbletPlatform platform)
        {
            (entrypointObject, platform) = QueryEntrypointFor(child).ResolveNow().FirstOrDefault();
            return entrypointObject;
        }

        public static bool TryGetRootObjectFor(Transform child, out GameObject entrypointObject)
            => TryGetEntrypointFor(child.gameObject, out entrypointObject, out _);
    }
}
