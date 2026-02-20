using Ablet.Repositories;
using UnityEditor;

namespace Ablet
{
    static class AbletStaticMenuItems
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += SetChecked;
            EditorSettingsRepository.Instance.OnChanged += SetChecked;
            EditorStateRepository.Instance.OnChanged += SetChecked;
        }

        [MenuItem("Tools/Ablet/Auto Open Console Window", false, 2)]
        static void AutoOpenConsoleWindow()
        {
            EditorSettingsRepository.Instance.Value.AutoOpenConsoleWindow = !EditorSettingsRepository.Instance.Value.AutoOpenConsoleWindow;
            EditorSettingsRepository.Instance.Save();
        }

        [MenuItem("Tools/Ablet/Enhance Inplace Preview", false, 60)]
        static void EnhancePreview()
        {
            EditorStateRepository.Instance.IsEnhancedInplacePreview = !EditorStateRepository.Instance.IsEnhancedInplacePreview;
        }

        static void SetChecked()
        {
            Menu.SetChecked("Tools/Ablet/Auto Open Console Window", EditorSettingsRepository.Instance.Value.AutoOpenConsoleWindow);
            Menu.SetChecked("Tools/Ablet/Enhance Inplace Preview", EditorStateRepository.Instance.IsEnhancedInplacePreview);
        }
    }
}
