using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ablet.API.Internal;
using Ablet.Debugging.View.Windows;
using Ablet.Models;
using Ablet.Planning;
using Ablet.Registries;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.Debugging.View.UIElements
{
    public class DebugLayerDependencyWindowView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/Debugging/View/Uxml/DebugLayerDependencyWindowView.uxml";

        readonly LayerDependencySet _layerDeps;
        readonly AbletLayer[] _layers;
        
        readonly ListView _layersList;
        readonly TextField _layerInfoField;

        public DebugLayerDependencyWindowView()
        {
            var layers = LayerRegistry.Instance.All();
            _layerDeps = LayerDependencySet.Build(layers); // this may populate dependency only layers
            _layers = LayerRegistry.Instance.All()
                .OrderBy(layer => layer.IsConcreteLayer ? 1 : typeof(IAbletPhase).IsAssignableFrom(layer.DefType) ? 0 : 2)
                .ThenBy(layer => layer.LayerPriority)
                .ThenBy(layer => layer.Id)
                .ToArray();

            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            var planButton = container.Q<Button>("planButton");
            planButton.clicked += OnPlan;
            var partialPlanButton = container.Q<Button>("partialPlanButton");
            partialPlanButton.clicked += OnPartialPlan;
            var copyLayerDepsButton = container.Q<Button>("copyLayerDepsButton");
            copyLayerDepsButton.clicked += OnCopyLayerDeps;
            _layersList = container.Q<ListView>("layersList");
            _layersList.itemsSource = _layers;
            _layersList.makeItem += () => new DebugAbletLayerView();
            _layersList.bindItem += (view, index) => ((DebugAbletLayerView)view).Draw(_layers[index]);
            void OnLayersListSelected(IEnumerable<object> selectedItems)
            {
                OnLayerSelected(selectedItems.OfType<AbletLayer>().FirstOrDefault());
            }
#if UNITY_2022_3_OR_NEWER
            _layersList.selectionChanged += OnLayersListSelected;
            _layersList.itemsChosen += OnLayersListSelected;
#else
            _layersList.onSelectionChange += OnLayersListSelected;
            _layersList.onItemsChosen += OnLayersListSelected;
#endif
            _layerInfoField = container.Q<TextField>("layerInfoField");
            hierarchy.Add(container);
        }

        void OnLayerSelected(AbletLayer? layer)
        {
            var layerInfo = layer == null
                ? ""
                : BuildLayerInfo(layer);
            _layerInfoField.SetValueWithoutNotify(layerInfo);
        }

        string BuildLayerInfo(AbletLayer layer)
        {
            var sb = new StringBuilder();
            sb.AppendLine(BuildLayerDepsInfo(_layerDeps.GetLayerDependency(layer)));
            return sb.ToString();
        }

        static string BuildLayerDepsInfo(LayerDependency layerDep)
        {
            var sb = new StringBuilder();
            sb.AppendLine(layerDep.Layer.Id);
            foreach (var dependency in layerDep.Dependencies)
            {
                sb.AppendLine($"< {dependency.Id}: {dependency.DisplayName}");
            }
            foreach (var dependent in layerDep.Dependents)
            {
                sb.AppendLine($"> {dependent.Id}: {dependent.DisplayName}");
            }
            return sb.ToString();
        }

        void OnPlan()
        {
            var plan = AvatarBuildPlanner.Plan(_layers, withContainerPass: true);
            DebugBuildPlanWindow.ShowWindow(plan);
        }

        void OnPartialPlan()
        {
            if (_layersList is { selectedItem: AbletLayer layer })
            {
                var plan = AvatarBuildPlanner.PartialPlan(_layers, layer, withContainerPass: true);
                DebugBuildPlanWindow.ShowWindow(plan);
            }
        }

        void OnCopyLayerDeps()
        {
            GUIUtility.systemCopyBuffer = _layerDeps.All()
                .Select(BuildLayerDepsInfo)
                .Aggregate(new StringBuilder(), (sb, pass) => sb.AppendLine(pass))
                .ToString();
        }
    }
}
