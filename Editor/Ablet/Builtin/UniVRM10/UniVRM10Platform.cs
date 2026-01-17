using System;
using System.IO;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Building;
using Ablet.EditorAPI.V1.Extensions.Platform;
using Ablet.Registries;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UniVRM10;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.UniVRM10
{
    [AbletExtension]
    class UniVRM10ExportExtension : IExportUIExtension
    {
        Type IAbletExtension.ForType => typeof(UniVRM10Platform);

        VisualElement IExportUIExtension.RenderExportUI(GameObject entrypointObject)
        {
            void OnBuild()
            {
                const string lastDirectoryPrefsKey = "net.kaikoga.ativ.VRM1.LastDirectory";
                var lastDirectory = PlayerPrefs.GetString(lastDirectoryPrefsKey, "");

                var filePath = EditorUtility.SaveFilePanel(
                    "Save VRM1.0 File",
                    lastDirectory,
                    $"{entrypointObject.name}.vrm",
                    ".vrm"); 
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    var directory = Path.GetDirectoryName(filePath) ?? "";
                    PlayerPrefs.SetString(lastDirectoryPrefsKey, directory);

                    try
                    {
                        var arguments = BuildArgument.FromEditModeAssetBuild(entrypointObject, PlatformRegistry.Instance.Get<UniVRM10Platform>(), true);
                        var buildResult = AbletFacade.BuildWithArguments(arguments);
                        VRM1FileExporter.ExportVRM1File(buildResult.GetComponent<Vrm10Instance>(), filePath);
                        AbletEditorUtil.OpenInExplorer(directory);
                        Object.DestroyImmediate(buildResult);
                    }
                    finally
                    {
                        AssetPersister.ClearTempAssets();
                    }
                }
            }

            return new Button(OnBuild)
            {
                text = "Export VRM1.0 Avatar"
            };
        }
    }
}
