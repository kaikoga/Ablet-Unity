using System;
using System.IO;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Building;
using Ablet.DataObjects;
using Ablet.EditorAPI.V1.Extensions.Platform;
using Ablet.Registries;
using Ablet.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UniVRM10;
using static Ablet.Loch.Tools.AbletLochTool;
using Button = Ablet.Loch.UIElements.AEditor.Button;
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
                    Tr("UniVRM10ExportExtension::Title"), // "Save VRM1.0 File"
                    lastDirectory,
                    $"{entrypointObject.name}.vrm",
                    ".vrm"); 
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    var directory = Path.GetDirectoryName(filePath) ?? "";
                    PlayerPrefs.SetString(lastDirectoryPrefsKey, directory);

                    var request = new UniVRM10ExportRequest();
                    var settings = ScriptableObject.CreateInstance<VRM10ExportSettings>();
                    try
                    {
                        var targetPlatform = PlatformRegistry.Instance.Get<UniVRM10Platform>();
                        var arguments = BuildArgumentBuilder.TargetsPlatform(entrypointObject, targetPlatform)
                            .AddInput(request)
                            .ForEditModeAssetBuild(true, BuildInitiationSourceMode.Ablet);
                        var buildResult = AbletFacade.BuildWithArguments(arguments);
                        ReadSetting(settings, request.Setting);
                        VRM1FileExporter.ExportVRM1File(buildResult.GetComponent<Vrm10Instance>(), settings, filePath);
                        AbletEditorUtil.OpenInExplorer(directory);
                        Object.DestroyImmediate(buildResult);
                    }
                    finally
                    {
                        Object.DestroyImmediate(settings);
                        AssetPersister.DelayClearTempAssets();
                    }
                }
            }

            return new Button(OnBuild)
            {
                loc = Loc("UniVRM10ExportExtension::exportButton"),
                text = "Export VRM1.0 Avatar..."
            }.LocalizeWith(typeof(UniVRM10ExportExtension));
        }

        static void ReadSetting(VRM10ExportSettings settings, UniVRM10ExportSetting requestSetting)
        {
            settings.MorphTargetUseSparse = requestSetting.morphTargetUseSparse;
            settings.FreezeMesh = requestSetting.freezeMesh;
            settings.FreezeMeshKeepRotation = requestSetting.freezeMeshKeepRotation;
            settings.FreezeMeshUseCurrentBlendShapeWeight = requestSetting.freezeMeshUseCurrentBlendShapeWeight;
        }
    }
}
