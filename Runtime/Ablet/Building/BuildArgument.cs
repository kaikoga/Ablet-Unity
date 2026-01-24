using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEngine;

namespace Ablet.Building
{
    public class BuildArgument : IBuildArgument
    {
        public GameObject EntrypointObject { get; }

        readonly AbletPlatform? _entrypointPlatform;
        readonly AbletPlatform _targetPlatform;
        readonly Catalyst _catalyst;
        readonly DatastoreRepository _inputs = new DatastoreRepository();

        IAbletPlatformHandle? IBuildArgument.EntrypointPlatform => _entrypointPlatform != null ? new AbletPlatformHandle(_entrypointPlatform) : null;
        IAbletPlatformHandle IBuildArgument.TargetPlatform => new AbletPlatformHandle(_targetPlatform);

        AssetGenerationMode IBuildArgument.WillCloneSceneObject => _catalyst.WillCloneSceneObject;
        AssetGenerationMode IBuildArgument.WillPersistGeneratedAssets => _catalyst.WillPersistGeneratedAssets;
        bool IBuildArgument.IsPartial => _catalyst.IsPartial;
        BuildInitiationSourceMode IBuildArgument.BuildInitiationSourceMode => _catalyst.BuildInitiationSourceMode;
        ObjectRetainMode IBuildArgument.ObjectRetainMode => _catalyst.ObjectRetainMode;

        internal PreviewMode PreviewMode => _catalyst.PreviewMode;
        internal bool IsObservable => _catalyst.IsObservable;
        internal ObjectRetainMode ObjectRetainMode => _catalyst.ObjectRetainMode;

        BuildArgument(GameObject entrypointObject, AbletPlatform? entrypointPlatform, AbletPlatform targetPlatform, Catalyst catalyst)
        {
            EntrypointObject = entrypointObject;
            _entrypointPlatform = entrypointPlatform;
            _targetPlatform = targetPlatform;
            _catalyst = catalyst;

            if (entrypointPlatform?.Id != targetPlatform.Id)
            {
                if (entrypointPlatform?.TryGetConverter(out _) == false
                    || !targetPlatform.TryGetConverter(out _))
                {
                    throw new ArgumentException($"Platform not convertible, entrypoint: <{entrypointPlatform?.DisplayName}> target: <{targetPlatform.DisplayName}>");
                }
            }
        }

        public BuildArgument AddInput<T>(T value)
        where T : class
        {
            _inputs.Add(value);
            return this;
        }

        bool IBuildArgument.TryGetInput<T>([MaybeNullWhen(false)] out T value) where T : class => _inputs.TryGet(out value);

        static BuildArgument FromGameObject(GameObject entrypointObject, AbletPlatform targetPlatform, Catalyst catalyst)
        {
            PlatformRegistry.Instance.TryGuessPlatform(entrypointObject, out var entrypointPlatform);
            return new BuildArgument(entrypointObject, entrypointPlatform, targetPlatform, catalyst);
        }
        
        public static BuildArgument FromEditModeAssetBuild(GameObject entrypointObject, AbletPlatform targetPlatform, bool willClone, BuildInitiationSourceMode buildInitiationSourceMode)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                WillCloneSceneObject = willClone ? AssetGenerationMode.Temporary : AssetGenerationMode.None,
                ObjectRetainMode = willClone ? ObjectRetainMode.None : ObjectRetainMode.RetainObjectId,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });
        public static BuildArgument FromAssetBuild(GameObject entrypointObject, AbletPlatform targetPlatform, bool willClone, BuildInitiationSourceMode buildInitiationSourceMode)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                WillCloneSceneObject = willClone ? AssetGenerationMode.Temporary : AssetGenerationMode.None,
                WillPersistGeneratedAssets = AssetGenerationMode.Temporary,
                ObjectRetainMode = willClone ? ObjectRetainMode.None : ObjectRetainMode.RetainObjectId,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });

        internal static BuildArgument FromInplacePreview(GameObject entrypointObject, AbletPlatform targetPlatform)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                PreviewMode = PreviewMode.InPlacePreview,
                IsObservable = true,
            });
        internal static BuildArgument FromManualApply(GameObject entrypointObject, AbletPlatform targetPlatform)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                WillCloneSceneObject = AssetGenerationMode.UserInitiated,
                WillPersistGeneratedAssets = AssetGenerationMode.UserInitiated
            });
        internal static BuildArgument FromApplyOnPlay(GameObject entrypointObject, AbletPlatform targetPlatform)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                ObjectRetainMode = ObjectRetainMode.RetainObjectId
            });
        internal static BuildArgument FromPartialBuild(GameObject entrypointObject, AbletPlatform targetPlatform, ObjectRetainMode objectRetainMode, BuildInitiationSourceMode buildInitiationSourceMode)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                IsPartial = true,
                ObjectRetainMode = objectRetainMode,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });
        internal static BuildArgument FromPartialAssetBuild(GameObject entrypointObject, AbletPlatform targetPlatform, ObjectRetainMode objectRetainMode, BuildInitiationSourceMode buildInitiationSourceMode)
            => FromGameObject(entrypointObject, targetPlatform, new Catalyst
            {
                IsPartial = true,
                WillPersistGeneratedAssets = AssetGenerationMode.Temporary,
                ObjectRetainMode = objectRetainMode,
                BuildInitiationSourceMode = buildInitiationSourceMode
            });

        struct Catalyst
        {
            public AssetGenerationMode WillCloneSceneObject;
            public AssetGenerationMode WillPersistGeneratedAssets;
            public bool IsPartial;
            public BuildInitiationSourceMode BuildInitiationSourceMode;
            public ObjectRetainMode ObjectRetainMode;

            public PreviewMode PreviewMode;
            public bool IsObservable;
        }
    }
}
