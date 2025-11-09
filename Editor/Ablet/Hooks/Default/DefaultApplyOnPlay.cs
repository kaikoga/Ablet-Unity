using System.Linq;
using Ablet.API;
using Ablet.Building;
using Ablet.EditorAPI;
using Ablet.EditorAPI.Attributes;
using Ablet.Planning;
using Ablet.Querying;
using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

namespace Ablet.Hooks.Default
{
    [AbletApplyOnPlay]
    public class DefaultApplyOnPlay : IAbletApplyOnPlay
    {
        string IAbletDefinitionBase.Id => "ablet.hooks.apply-on-play.default";
        string IAbletDefinitionBase.DisplayName => "Apply on Play";
        int IAbletDefinitionBase.Priority => int.MaxValue;

        bool IAbletApplyOnPlay.Available => true;

        void IAbletApplyOnPlay.OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            // do nothing
        }

        void IAbletApplyOnPlay.OnRuntimeInitializeOnLoad()
        {
            AbletAwaker.OnAbletAwake = ApplyOnPlay;
            var awaker = new GameObject("AbletAwaker");
            awaker.AddComponent<AbletAwaker>();
            Object.DontDestroyOnLoad(awaker);
        }

        void ApplyOnPlay()
        {
            foreach (var platform in PlatformRepository.Instance.All().Where(platform => platform.ApplyOnPlay))
            {
                // TODO: use AQuery.GetEntrypoints()
                // naively skip inactive avatars
                foreach (var marker in AQuery.GetComponents(platform.EntryPointComponentType).Query())
                {
                    var arguments = BuildArgument.FromApplyOnPlay(platform, marker.gameObject);
                    var plan = BuildPlanner.Plan(LayerRepository.Instance.All());
                    var process = BuildProcess.FromPlan(plan, arguments);
                    process.Build();
                }
            }
        }
    }
}
