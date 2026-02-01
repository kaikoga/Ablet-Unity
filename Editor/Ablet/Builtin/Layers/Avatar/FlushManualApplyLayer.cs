using System;
using System.IO;
using System.Linq;
using Ablet.API;
using Ablet.API.Internal;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Building;
using Ablet.Building;
using Ablet.Building.Ephemeral;
using Ablet.Builtin.Utils;
using Ablet.Utils;
using UnityEditor;

namespace Ablet.Builtin.Layers.Avatar
{
    [AbletLayer]
    class FlushManualApplyLayer : IAbletSpecialLayer
    {
        string IAbletDefinition.Id => BuiltinLayerIds.Avatar.FlushManualApply;
        string IAbletDefinition.DisplayName => "Flush Manual Apply";
        void IAbletLayer.Configure(IDependencyConfigurator config)
        {
            config.AddDependency<BeforeLayer<AvatarBuildingRootLayer>>();
        }

        int IAbletSpecialLayer.LayerPriority => int.MinValue;
        string IAbletSpecialLayer.IdForPriority => "";
        int IAbletSpecialLayer.InnerPriority => int.MinValue;
        bool IAbletSpecialLayer.IsConcreteLayer => true;

        AbletProcedure? IAbletLayer.ToProcedure(IBuildArgument argument)
        {
            return argument.WillCloneSceneObject switch
            {
                AssetGenerationMode.None => null,
                AssetGenerationMode.Temporary => null,
                AssetGenerationMode.UserInitiated => new FlushManualApplyProcedure(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    class FlushManualApplyProcedure : AbletBuildProcedure
    {
        const string OptOutDecisionKey = "Ablet.FlushManualApply.OptOut";

        static readonly TimeSpan ClearManualAssetsDelay = TimeSpan.FromHours(12);

        public override void Process(IBuildContext context)
        {
            var maxDuration = AssetPersister.GetManualAssetPaths()
                .Select(path => DateTime.Now - File.GetCreationTime(path))
                .Max();
            if (maxDuration > ClearManualAssetsDelay)
            {
                if (EditorUtility.GetDialogOptOutDecision(DialogOptOutDecisionType.ForThisSession, OptOutDecisionKey))
                {
                    return;
                }
                if (EditorUtility.DisplayDialog(
                        "Ablet: Manual Apply",
                        "You have old outputs of Manual Apply, do you wish to clean up?",
                        "Delete Manual Apply outputs",
                        "Do not clear (for this session)"))
                {
                    AssetPersister.ClearManualAssets();
                }
                else
                {
                    EditorUtility.SetDialogOptOutDecision(DialogOptOutDecisionType.ForThisSession, OptOutDecisionKey, true);
                }
            }
        }
    }
}
