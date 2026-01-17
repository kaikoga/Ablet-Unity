using UnityEngine;

namespace Ablet.Previewing.Internal.Presentation
{
    public interface IInplacePreviewPresenter
    {
        GameObject? Avatar { get; }
        void EndPreview();
        GameObject StartPreview(GameObject originalAvatar);
    }
}