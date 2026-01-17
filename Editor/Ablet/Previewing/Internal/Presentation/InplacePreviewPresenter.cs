using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ablet.Previewing.Internal.Presentation
{
    class InplacePreviewPresenter : IInplacePreviewPresenter
    {
        public static readonly IInplacePreviewPresenter Instance = new InplacePreviewPresenter();

        public GameObject? Avatar { get; private set; }

        public void EndPreview()
        {
            SceneVisibilityManager.instance.ExitIsolation();
            if (Avatar)
            {
                Object.DestroyImmediate(Avatar);
            }
        }

        public GameObject StartPreview(GameObject originalAvatar)
        {
            Avatar = Object.Instantiate(originalAvatar);
            SceneVisibilityManager.instance.Isolate(Avatar.gameObject, true);
            Avatar.hideFlags = HideFlags.HideAndDontSave;
            return Avatar;
        }
    }
}
