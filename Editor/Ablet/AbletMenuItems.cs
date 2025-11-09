using System.Linq;
using System.Text;
using Ablet.Planning;
using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

namespace Ablet
{
    public static class AbletMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
        }

        [MenuItem("Tools/Ablet/Debug/Show Build Plan", false, 0)]
        static void DebugShowBuildPlan()
        {
            var plan = BuildPlanner.Plan(LayerRepository.Instance.All());
            Debug.LogError(plan.Aggregate(new StringBuilder(), (sb, pass) =>
            {
                sb.AppendLine($"{pass.Layer.Id}: {pass.Layer.DisplayName}");
                return sb;
            }));
        }
        
        [MenuItem("Tools/Ablet/Debug/Show Layer Dependency", false, 0)]
        static void DebugShowLayerDependency()
        {
            var layerDeps = LayerDependencySet.Build(LayerRepository.Instance.All());
            Debug.LogError(layerDeps.All().Aggregate(new StringBuilder(), (sb, layerDep) =>
            {
                sb.AppendLine(layerDep.Layer.Id);
                foreach (var dependency in layerDep.Dependencies)
                {
                    sb.AppendLine($"< {dependency.Id}: {dependency.DisplayName}");
                }
                foreach (var dependent in layerDep.Dependents)
                {
                    sb.AppendLine($"> {dependent.Id}: {dependent.DisplayName}");
                }
                return sb;
            }));
        }
        
        [MenuItem("Tools/Ablet/Manual Export", true, 1)]
        static bool ValidateManualExportGameObject()
        {
            if (Selection.activeGameObject == null)
            {
                return false;
            }
            var platform = PlatformRepository.Instance.GuessPlatform(Selection.activeGameObject);
            return platform != null;
        }

        [MenuItem("Tools/Ablet/Manual Export", false, 1)]
        static void ManualExportGameObject()
        {
            AbletFacade.ManualExportGameObject(Selection.activeGameObject);
        }

        [MenuItem("Tools/Ablet/Apply on Play", false, 10)]
        static void ApplyOnPlay()
        {
            EditorSettingsRepository.Instance.Value.ApplyOnPlay = !EditorSettingsRepository.Instance.Value.ApplyOnPlay;
            EditorSettingsRepository.Instance.Save();
            SetChecked();
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Apply on Play", EditorSettingsRepository.Instance.Value.ApplyOnPlay);
        }
    }
}
