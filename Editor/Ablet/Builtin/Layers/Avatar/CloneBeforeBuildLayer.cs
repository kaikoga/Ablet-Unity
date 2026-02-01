using System;
using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Builtin.Utils;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class CloneBeforeBuildLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.CloneBeforeBuild;
        string IAbletDefinition.DisplayName => "Clone Before Build";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<AvatarBuildingRootLayer>>();
        }

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => 0;
        bool IAbletSpecialLayer.IsConcreteLayer => true;

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            if (EditorUtility.IsPersistent(argument.EntrypointObject))
            {
                return new CloneBeforeBuildProcedure(false);
            }
            return argument.WillCloneSceneObject switch
            {
                AssetGenerationMode.None => null,
                AssetGenerationMode.Temporary => new CloneBeforeBuildProcedure(false),
                AssetGenerationMode.UserInitiated => new CloneBeforeBuildProcedure(true),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    class CloneBeforeBuildProcedure : AbletBuildProcedure
    {
        readonly bool _asUserInitiated;

        public CloneBeforeBuildProcedure(bool asUserInitiated)
        {
            _asUserInitiated = asUserInitiated;
        }
        
        public override void Process(IBuildContext context)
        {
            var clone = Object.Instantiate(context.CurrentRootObject);
            Undo.RegisterCreatedObjectUndo(clone, "Ablet: Prepare Manual Apply");
            if (_asUserInitiated)
            {
                var position = clone.transform.position;
                position.z += 2;
                clone.transform.position = position;
            }
            else
            {
                clone.name = context.CurrentRootObject.name;
            }
            context.SetCurrentRootObject(clone);
        }
    }
}
