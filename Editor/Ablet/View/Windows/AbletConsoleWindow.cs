using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.Models.Serialized;
using Ablet.Querying;
using Ablet.Repositories;
using Ablet.View.UIElements;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Ablet.View.Windows
{
    class AbletConsoleWindow : EditorWindow
    {
        [InitializeOnLoadMethod]
        static void OnInitializeOnLoad()
        {
            BuildReportRepository.Instance.Changed += OnBuildReport;
        }

        static void OnBuildReport(bool userNotify, SerializedEntrypointReference? withEntrypointRef)
        {
            if (userNotify && EditorSettingsRepository.Instance.Value.AutoOpenConsoleWindow)
            {
                ShowWindow();
            }
        }

        [MenuItem("Tools/Ablet/Ablet Console Window", false, 1)]
        static void ShowWindow()
        {
            (GetWindow<AbletConsoleWindow>() ?? CreateInstance<AbletConsoleWindow>()).Show();
        }
        
        AbletConsoleWindowView? _view;

        SerializedEntrypointReference[] _currentEntrypoints = {};

        void CreateGUI()
        {
            titleContent = new GUIContent("Ablet Console Window");
            minSize = new Vector2(600f, 400f);
            _view = new AbletConsoleWindowView();
            rootVisualElement.Add(_view);

            void OnEntrypointListSelected(IEnumerable<object> selectedItems)
            {
                OnEntrypointSelected(selectedItems.OfType<SerializedEntrypointReference>().ToArray());
            }
#if UNITY_2022_3_OR_NEWER
            _view.EntrypointList.selectionChanged += OnEntrypointListSelected;
            _view.EntrypointList.itemsChosen += OnEntrypointListSelected;
#else
            _view.EntrypointList.onSelectionChange += OnEntrypointListSelected;
            _view.EntrypointList.onItemsChosen += OnEntrypointListSelected;
#endif
            _view.SceneEntrypointsPopup.RegisterValueChangedCallback(evt => OnSceneEntrypointSelected(evt.newValue));
            _view.EntrypointObjectField.RegisterValueChangedCallback(evt => ManualReport(evt.newValue as GameObject));
            _view.ClearReportsButton.clicked += OnClearReportsClicked;
            BuildReportRepository.Instance.Changed += OnBuildReportChanged;
            BuildReporterStateRepository.Instance.Changed += RefreshBuildReporterState;
            BuildReporterSettingsUIBroker.Instance.OnShowSettingsUIRequested += OnShowSettingsUIRequested;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneClosed += OnSceneClosed;
            RefreshBuildReporterState();
            RefreshSceneAvatarsPopup();
            Refresh();
        }
        void OnDisable()
        {
            BuildReportRepository.Instance.Changed -= OnBuildReportChanged;
            BuildReporterStateRepository.Instance.Changed -= RefreshBuildReporterState;
            BuildReporterSettingsUIBroker.Instance.OnShowSettingsUIRequested -= OnShowSettingsUIRequested;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneClosed -= OnSceneClosed;
        }

        void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            Refresh();
        }

        void OnSceneClosed(Scene scene)
        {
            Refresh();
        }

        void OnHierarchyChanged()
        {
            RefreshSceneAvatarsPopup();
        }

        void OnEntrypointSelected(SerializedEntrypointReference[] selections)
        {
            if (selections.Length == 0)
            {
                return;
            }
            _currentEntrypoints = selections;
            Refresh();
        }

        void OnClearReportsClicked()
        {
            BuildReportRepository.Instance.Clear();
        }

        void OnSceneEntrypointSelected(GameObject avatarRootObject)
        {
            if (_view != null)
            {
                if (_view.SceneEntrypointsPopup.index < 0)
                {
                    return;
                }
                ManualReport(avatarRootObject);
                _view.SceneEntrypointsPopup.index = -1;
            }
        }

        void ManualReport(GameObject? avatarRootObject)
        {
            _currentEntrypoints = avatarRootObject != null
                ? new []{ SerializedEntrypointReference.From(avatarRootObject) }
                : Array.Empty<SerializedEntrypointReference>();
            Refresh();
        }

        void OnBuildReportChanged(bool notifyUser, SerializedEntrypointReference? withEntrypointRef)
        {
            if (notifyUser && withEntrypointRef != null)
            {
                _currentEntrypoints = new[] { withEntrypointRef };
            }
            Refresh();
        }

        void RefreshBuildReporterState()
        {
            _view?.BuildReporterList.Draw(BuildReporterStateRepository.Instance.All().ToList());
            Refresh();
        }

        void Refresh()
        {
            if (_view != null)
            {
                _view.EntrypointList.itemsSource = BuildReportRepository.Instance.EntrypointReferences().ToList();
                _view.DrawBuildReportLists(BuildReportRepository.Instance.ForEntrypoints(_currentEntrypoints));
            }
        }

        void OnShowSettingsUIRequested(VisualElement settingsUI)
        {
            _view?.DrawSettingsUI(settingsUI);
        }

        void RefreshSceneAvatarsPopup()
        {
            if (_view != null)
            {
                _view.SceneEntrypointsPopup.choices = AbletFacade.QuerySceneEntrypoints(true)
                    .Select(gp => gp.gameObject)
                    .ResolveNow().ToList();
                _view.SceneEntrypointsPopup.SetEnabled(!Application.isPlaying);
            }
        }
    }
}
