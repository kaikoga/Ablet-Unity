using System;
using System.IO;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Building;
using Ablet.Building.Ephemeral;
using Ablet.EditorAPI.V1.Extensions.Platform;
using Ablet.Registries;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.UniVRM
{
    [AbletExtension]
    class UniVRMExportExtension : IExportUIExtension
    {
        Type IAbletExtension.ForType => typeof(UniVRMPlatform);

        VisualElement IExportUIExtension.RenderExportUI(GameObject entrypointObject)
        {
            void OnBuild()
            {
                const string lastDirectoryPrefsKey = "net.kaikoga.ativ.VRM0.LastDirectory";
                var lastDirectory = PlayerPrefs.GetString(lastDirectoryPrefsKey, "");

                var filePath = EditorUtility.SaveFilePanel("Save VRM0.x File", lastDirectory, $"{entrypointObject.name}.vrm", ".vrm");
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    var directory = Path.GetDirectoryName(filePath) ?? "";
                    PlayerPrefs.SetString(lastDirectoryPrefsKey, directory);

                    try
                    {
                        var arguments = BuildArgument.FromEditModeAssetBuild(entrypointObject, PlatformRegistry.Instance.Get<UniVRMPlatform>(), true, BuildInitiationSourceMode.Ablet);
                        var buildResult = AbletFacade.BuildWithArguments(arguments);
                        VRM0FileExporter.ExportVRM0File(buildResult.GetComponent<VRMMeta>(), filePath);
                        AbletEditorUtil.OpenInExplorer(directory);
                        Object.DestroyImmediate(buildResult);
                    }
                    finally
                    {
                        AssetPersister.DelayClearTempAssets();
                    }
                }
            }
            
            return new Button(OnBuild)
            {
                text = "Export VRM0.x Avatar"
            };
        }
        
        
    }
}
