using System;
using System.Diagnostics.CodeAnalysis;
using Ablet.API.V1;
using Ablet.API.V1.Building;
using Ablet.Models;
using Ablet.Repositories;
using UnityEngine;

namespace Ablet.Building
{
    public class BuildArgument : IBuildArgument
    {
        public GameObject EntrypointObject { get; }

        readonly AbletPlatform? _entrypointPlatform;
        readonly AbletPlatform _targetPlatform;
        readonly AbletSubplatform _targetSubplatform;
        readonly Catalyst _catalyst;
        readonly DatastoreRepository _inputs = new DatastoreRepository();

        IAbletPlatformHandle? IBuildArgument.EntrypointPlatform => _entrypointPlatform != null ? new AbletPlatformHandle(_entrypointPlatform) : null;
        IAbletPlatformHandle IBuildArgument.TargetPlatform => new AbletPlatformHandle(_targetPlatform);
        IAbletPlatformHandle IBuildArgument.TargetSubplatform => new AbletPlatformHandle(_targetSubplatform);

        string IBuildArgument.CatalystId => _catalyst.Id;
        AssetGenerationMode IBuildArgument.WillCloneSceneObject => _catalyst.WillCloneSceneObject;
        AssetGenerationMode IBuildArgument.WillPersistGeneratedAssets => _catalyst.WillPersistGeneratedAssets;
        bool IBuildArgument.IsPartial => _catalyst.IsPartial;
        BuildInitiationSourceMode IBuildArgument.BuildInitiationSourceMode => _catalyst.BuildInitiationSourceMode;

        internal string CatalystId => _catalyst.Id;
        internal PreviewMode PreviewMode => _catalyst.PreviewMode;
        internal bool IsObservable => _catalyst.IsObservable;
        internal ObjectRetainMode ObjectRetainMode => _catalyst.ObjectRetainMode;

        internal BuildArgument(
            GameObject entrypointObject,
            AbletPlatform? entrypointPlatform,
            AbletPlatform targetPlatform,
            AbletSubplatform targetSubplatform,
            Catalyst catalyst)
        {
            EntrypointObject = entrypointObject;
            _entrypointPlatform = entrypointPlatform;
            _targetPlatform = targetPlatform;
            _targetSubplatform = targetSubplatform;
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
    }
}
