using Ablet.API.V1;
using Ablet.Models;
using Ablet.Registries;
using UnityEngine;

namespace Ablet.Building
{
    public class BuildArgumentBuilder
    {
        readonly GameObject _entrypointObject;
        readonly AbletPlatform? _entrypointPlatform;
        readonly AbletPlatform _targetPlatform;

        public BuildArgumentBuilder(GameObject entrypointObject, AbletPlatform targetPlatform)
        {
            _entrypointObject = entrypointObject;
            PlatformRegistry.Instance.TryGuessPlatform(entrypointObject, out _entrypointPlatform);
            _targetPlatform = targetPlatform;
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

        BuildArgument Build(Catalyst catalyst)
        {
            return new BuildArgument(_entrypointObject, _entrypointPlatform, _targetPlatform, catalyst);
        }
    }
}
