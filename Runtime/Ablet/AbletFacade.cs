using Ablet.API;
using Ablet.Building;
using Ablet.Planning;
using Ablet.Repositories;
using UnityEngine;

namespace Ablet
{
    public static class AbletFacade
    {
        public static void ManualExportGameObject(GameObject entrypointObject)
        {
            var platform = PlatformRepository.Instance.GuessPlatform(entrypointObject);
            ManualExportGameObject(entrypointObject, platform);
        }

        public static void ManualExportGameObject(GameObject entrypointObject, IAbletPlatform platform)
        {
            var arguments = BuildArgument.FromManualExport(platform, entrypointObject);
            var plan = BuildPlanner.Plan(LayerRepository.Instance.All());
            var process = BuildProcess.FromPlan(plan, arguments);
            process.Build();
        }
    }
}
