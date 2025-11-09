using Ablet.API;
using Ablet.API.Internal;
using Ablet.Builtin;
using Ablet.Repositories.Internal;
using UnityEngine;

namespace Ablet.Building
{
    public class BuildArgument
    {
        public readonly IAbletPlatform Platform;
        public readonly GameObject EntrypointObject;

        readonly IAbletCatalyst _catalyst;

        public bool IsUserInitiatedAction => _catalyst.IsUserInitiatedAction;
        public bool WillPersistGeneratedAssets => _catalyst.WillPersistGeneratedAssets;
        public bool IsExternalSemantics => _catalyst.IsExternalSemantics;

        BuildArgument(IAbletPlatform platform, GameObject entrypointObject, IAbletCatalyst catalyst)
        {
            Platform = platform;
            EntrypointObject = entrypointObject;
            _catalyst = catalyst;
        }

        public static BuildArgument FromManualExport(IAbletPlatform platform, GameObject entrypointObject)
            => FromGameObject<Catalysts.ManualExport>(platform, entrypointObject);
        public static BuildArgument FromApplyOnPlay(IAbletPlatform platform, GameObject entrypointObject)
            => FromGameObject<Catalysts.ApplyOnPlay>(platform, entrypointObject);
        public static BuildArgument FromPartialBuild(IAbletPlatform platform, GameObject entrypointObject)
            => FromGameObject<Catalysts.PartialBuild>(platform, entrypointObject);

        static BuildArgument FromGameObject<T>(IAbletPlatform platform, GameObject entrypointObject)
        where T : IAbletCatalyst
            => new BuildArgument(platform, entrypointObject, CatalystRepository.Instance.Get<T>());
    }
}
