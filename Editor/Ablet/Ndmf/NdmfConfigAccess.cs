using System;
using System.Reflection;
using nadena.dev.ndmf.config;
using nadena.dev.ndmf.preview;
using UnityEngine;

namespace Ablet.Ndmf
{
    public static class NdmfConfigAccess
    {
        public static bool NdmfApplyOnPlay
        {
            get => Config.ApplyOnPlay;
            set => Config.ApplyOnPlay = value;
        }

        public static bool NdmfApplyOnBuild
        {
            get => Config.ApplyOnBuild;
            set => Config.ApplyOnBuild = value;
        }

        public static void TrySetNdmfEnablePreviews(bool enable)
        {
            // just try because we are on reflections, don't be too serious...
            try
            {
                var enablePreviewsUI = typeof(NDMFPreview).GetProperty("EnablePreviewsUI", BindingFlags.Static | BindingFlags.NonPublic);
                enablePreviewsUI?.SetValue(null, enable);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static bool IsNdmfOnAbletAvailable()
        {
            return !NdmfApplyOnPlay;
        }
    }
}
