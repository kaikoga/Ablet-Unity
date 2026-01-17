using UnityEngine;

namespace Ablet.API.V1.Extensions.Platform
{
    public interface IConvertiblePlatformExtension : IAbletExtension
    {
        void MarkAsAvatarRoot(GameObject avatarRoot);
        void MaterializeAsAvatarRoot(GameObject avatarRoot);
        void UnmarkAsAvatarRoot(GameObject avatarRoot);
    }
}
