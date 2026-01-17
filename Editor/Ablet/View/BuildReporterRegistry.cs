using System;
using System.Collections.Generic;
using System.Linq;
using Ablet.API;
using Ablet.EditorAPI.V1;
using Ablet.EditorAPI.V1.Attributes;
using Ablet.Models;
using Ablet.Registries.Base;
using Ablet.Utils.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ablet.View
{
    class BuildReporterRegistry : IdModelRegistryBase<IAbletBuildReporter, AbletBuildReporter>
    {
        public static readonly BuildReporterRegistry Instance = new BuildReporterRegistry();

        BuildReporterRegistry()
        {
            Collect(new ModelCollector<IAbletBuildReporter, AbletBuildReporterAttribute, AbletBuildReporter>(def => new AbletBuildReporter(def)));
        }

        public AbletBuildReporter For(IAbletSerializedBuildReportPayload payload)
        {
            return Unordered()
                .FirstOrDefault(reporter => reporter.ForType.IsInstanceOfType(payload))
                ?? MissingReporter.Instance;
        }

        public override IEnumerable<AbletBuildReporter> All() => Unordered().OrderBy(ext => ext.DisplayName);
    }

    class AbletBuildReporter : IAbletIdModelBase, IAbletBuildReporter
    {
        readonly IAbletBuildReporter _def;

        public Type DefType => _def.GetType();
        public string Id => _def.Id;
        public string DisplayName => _def.DisplayName;
        public Type ForType => _def.ForType;
        public StyleSheet? StyleSheet => _def.StyleSheet;
        public VisualElement? Render(IAbletSerializedBuildReportPayload payload, GameObject? entrypointObject) => _def.Render(payload, entrypointObject);
        public AbletBuildReporter(IAbletBuildReporter def) => _def = def;

        public bool IsEnabled => BuildReporterStateRepository.Instance.GetEnabled(Id);
    }
}
