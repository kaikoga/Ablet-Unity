using Ablet.API.V1;
using Ablet.Models;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEngine;

namespace Ablet.Building
{
    public class BuildArgumentBuilder
    {
        readonly GameObject _entrypointObject;
        readonly AbletPlatform? _entrypointPlatform;
        readonly AbletPlatform _targetPlatform;
        readonly AbletSubplatform _targetSubplatform;
        readonly DatastoreRepository _inputs = new DatastoreRepository();

        BuildArgumentBuilder(GameObject entrypointObject, AbletPlatform targetPlatform, AbletSubplatform targetSubplatform)
        {
            _entrypointObject = entrypointObject;
            PlatformRegistry.Instance.TryGuessPlatform(entrypointObject, out _entrypointPlatform);
            _targetPlatform = targetPlatform;
            _targetSubplatform = targetSubplatform;
        }

        public static BuildArgumentBuilder TargetsPlatform(GameObject entrypointObject, AbletPlatform targetPlatform)
        {
            var targetSubplatform = SubplatformRegistry.Instance.GuessSubplatform(entrypointObject, targetPlatform);
            return new BuildArgumentBuilder(entrypointObject, targetPlatform, targetSubplatform);
        }

        public static BuildArgumentBuilder TargetsSubplatform(GameObject entrypointObject, AbletSubplatform targetSubplatform)
        {
            var targetPlatform = targetSubplatform.Platform;
            return new BuildArgumentBuilder(entrypointObject, targetPlatform, targetSubplatform);
        }

        public BuildArgument ForEditModeAssetBuild(bool willClone, BuildInitiationSourceMode buildInitiationSourceMode)
            => Build(new Catalyst("Ablet.Catalyst.EditModeAssetBuild")
            {
                WillCloneSceneObject = willClone ? AssetGenerationMode.Temporary : AssetGenerationMode.None,
                ObjectRetainMode = willClone ? ObjectRetainMode.None : ObjectRetainMode.RetainObjectId,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });
        public BuildArgument ForAssetBuild(bool willClone, BuildInitiationSourceMode buildInitiationSourceMode)
            => Build(new Catalyst("Ablet.Catalyst.AssetBuild")
            {
                WillCloneSceneObject = willClone ? AssetGenerationMode.Temporary : AssetGenerationMode.None,
                WillPersistGeneratedAssets = AssetGenerationMode.Temporary,
                ObjectRetainMode = willClone ? ObjectRetainMode.None : ObjectRetainMode.RetainObjectId,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });

        internal BuildArgument ForInplacePreview()
            => Build(new Catalyst("Ablet.Catalyst.InplacePreview")
            {
                PreviewMode = PreviewMode.InPlacePreview,
                IsObservable = true,
            });
        internal BuildArgument ForManualApply()
            => Build(new Catalyst("Ablet.Catalyst.ManualApply")
            {
                WillCloneSceneObject = AssetGenerationMode.UserInitiated,
                WillPersistGeneratedAssets = AssetGenerationMode.UserInitiated
            });
        internal BuildArgument ForApplyOnPlay()
            => Build(new Catalyst("Ablet.Catalyst.ApplyOnPlay")
            {
                ObjectRetainMode = ObjectRetainMode.RetainObjectId
            });
        internal BuildArgument ForPartialBuild(ObjectRetainMode objectRetainMode, BuildInitiationSourceMode buildInitiationSourceMode)
            => Build(new Catalyst("Ablet.Catalyst.PartialBuild")
            {
                IsPartial = true,
                ObjectRetainMode = objectRetainMode,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });
        internal BuildArgument ForPartialAssetBuild(ObjectRetainMode objectRetainMode, BuildInitiationSourceMode buildInitiationSourceMode)
            => Build(new Catalyst("Ablet.Catalyst.PartialAssetBuild")
            {
                IsPartial = true,
                WillPersistGeneratedAssets = AssetGenerationMode.Temporary,
                ObjectRetainMode = objectRetainMode,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });

        public BuildArgumentBuilder AddInput<T>(T value)
            where T : class
        {
            _inputs.Add(value);
            return this;
        }

        BuildArgument Build(Catalyst catalyst)
        {
            return new BuildArgument(
                _entrypointObject,
                _entrypointPlatform,
                _targetPlatform,
                _targetSubplatform,
                catalyst,
                _inputs);
        }
    }
}
