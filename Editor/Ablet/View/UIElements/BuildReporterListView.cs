using System.Collections.Generic;
using Ablet.EditorAPI.V1.Extensions.BuildReporter;
using Ablet.Models.Extensions;
using Silksprite.Loch.UIElements.Tools;
using UnityEditor;
using UnityEngine.UIElements;

namespace Ablet.View.UIElements
{
    class BuildReporterListView : VisualElement
    {
        readonly ListView _listView;
        readonly List<BuildReporterState> _itemsSource = new List<BuildReporterState>();

        public BuildReporterListView()
        {
            _listView = new ListView
            {
                itemsSource = _itemsSource,
                selectionType = SelectionType.None
            };
            _listView.makeItem += () => new BuildReporterStateView();
            _listView.bindItem += (visualElement, index) =>
            {
                var view = (BuildReporterStateView)visualElement;
                view.Draw(_itemsSource[index]);
            };
            hierarchy.Add(_listView);
        }

        public void Draw(List<BuildReporterState> reporterStates)
        {
            _itemsSource.Clear();
            _itemsSource.AddRange(reporterStates);
            _listView.RefreshItems();
        }

        public new class UxmlFactory : UxmlFactory<BuildReporterListView, UxmlTraits>
        {
        }
    }

    class BuildReporterStateView : VisualElement
    {
        const string UxmlPath = "Packages/net.kaikoga.ablet/Editor/Ablet/View/Uxml/BuildReporterStateView.uxml";
        
        readonly Toggle _enabledToggle;
        readonly Label _displayNameLabel;
        readonly Button _settingsButton;

        string? _stateId;
        ISettingsUIExtension? _extensionDef;

        public BuildReporterStateView()
        {
            var container = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath).CloneTree();
            container.Localize<BuildReporterStateView>();

            container.style.flexGrow = 1;
            _enabledToggle = container.Q<Toggle>("enabledToggle");
            _enabledToggle.RegisterValueChangedCallback(evt => OnChangeEnabled(evt.newValue));
            _displayNameLabel = container.Q<Label>("displayNameLabel");
            _settingsButton = container.Q<Button>("settingsButton");
            _settingsButton.clickable.clicked += OnSettingsButtonClicked;
            hierarchy.Add(container);
        }

        public void Draw(BuildReporterState state)
        {
            _stateId = state.id;
            _enabledToggle.SetValueWithoutNotify(state.enabled);
            if (BuildReporterRegistry.Instance.TryGetById(state.id, out var reporter))
            {
                _displayNameLabel.text = reporter.DisplayName;
                _settingsButton.style.display = reporter.TryGetExtensionDef<ISettingsUIExtension>(out _extensionDef) ? DisplayStyle.Flex : DisplayStyle.None;
            }
            else
            {
                _displayNameLabel.text = state.id;
                _settingsButton.style.display = DisplayStyle.None;
            }
        }

        void OnChangeEnabled(bool value)
        {
            BuildReporterStateRepository.Instance.SetEnabled(_stateId!, value);
        }

        void OnSettingsButtonClicked()
        {
            if (_extensionDef?.RenderSettingsUI() is { } settingsUI)
            {
                BuildReporterSettingsUIBroker.Instance.RequestShowSettingsUI(settingsUI);
            }
        }
    } 
}
