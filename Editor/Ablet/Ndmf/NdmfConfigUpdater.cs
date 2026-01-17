using Ablet.Repositories;
using nadena.dev.ndmf.config;
using UnityEditor;

namespace Ablet.Ndmf
{
    public static class NdmfConfigUpdater
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            UpdateNdmfConfig();
        }

        public static void UpdateNdmfConfig()
        {
            // Prevent running both NDMF Apply on Play and Ablet Apply on Play and letting some plugins run twice
            if (EditorSettingsRepository.Instance.Value.ApplyOnPlay)
            {
                Config.ApplyOnPlay = false;
            }
            // Prevent running both NDMF Apply on Build and Ablet Apply on Platform Build and letting some plugins run twice
            if (EditorSettingsRepository.Instance.Value.ApplyOnPlatformBuild)
            {
                Config.ApplyOnBuild = false;
            }
            // NDMF preview can work along with Ablet Inplace Preview
            // NdmfConfigAccess.TrySetNdmfEnablePreviews(false);
        }

        public static void RevertNdmfConfig()
        {
            Config.ApplyOnPlay = true;
            Config.ApplyOnBuild = true;
            // NdmfConfigAccess.TrySetNdmfEnablePreviews(true);
        }
    }
}
