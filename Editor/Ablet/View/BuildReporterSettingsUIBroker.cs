using System;
using UnityEngine.UIElements;

namespace Ablet.View
{
    class BuildReporterSettingsUIBroker
    {
        public static readonly BuildReporterSettingsUIBroker Instance = new BuildReporterSettingsUIBroker();

        public event Action<VisualElement>? OnShowSettingsUIRequested;

        public void RequestShowSettingsUI(VisualElement settingsUI)
        {
            OnShowSettingsUIRequested?.Invoke(settingsUI);            
        }
    }
}
