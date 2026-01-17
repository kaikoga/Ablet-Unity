using System;
using System.Collections.Generic;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Extensions.Platform;
using Ablet.Utils;
using UniHumanoid;
using UnityEngine;
using UniVRM10;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.UniVRM10
{
    [AbletPlatform]
    public class UniVRM10Platform : IAbletPlatform
    {
        string IAbletDefinition.Id => BuiltinPlatformIds.UniVRM10;
        string IAbletDefinition.DisplayName => "UniVRM VRM1";
        int IAbletPlatform.Priority => 0;

        Type IAbletPlatform.EntrypointComponentType => typeof(Vrm10Instance);
        bool IAbletPlatform.FilterEntrypoint(Component component) => true;
    }
    
    [AbletExtension]
    class UniVRM10Extension : IApplyOnPlaySupportExtension, IConvertiblePlatformExtension
    {
        Type IAbletExtension.ForType => typeof(UniVRM10Platform);

        void IConvertiblePlatformExtension.MarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (!avatarRoot.TryGetComponent(out Vrm10Instance vrm10Instance))
            {
                vrm10Instance = avatarRoot.AddComponent<Vrm10Instance>();
            }

            // Always provide data structure
            if (!vrm10Instance.Vrm)
            {
                var vrm10Object = ScriptableObject.CreateInstance<VRM10Object>();
                var meta =  vrm10Object.Meta;
                meta.Name = AbletRuntimeUtil.GuessOriginalAvatarName(avatarRoot);
                meta.Authors = new List<string> { AbletRuntimeUtil.VrmAuthor };
                meta.Version = AbletRuntimeUtil.VrmVersion;
                vrm10Instance.Vrm = vrm10Object;
            }
            if (!avatarRoot.TryGetComponent(out Humanoid _))
            {
                var humanoid = avatarRoot.AddComponent<Humanoid>();
                humanoid.AssignBonesFromAnimator();
            }
        }

        void IConvertiblePlatformExtension.MaterializeAsAvatarRoot(GameObject avatarRoot)
        {
            // do nothing
        }

        void IConvertiblePlatformExtension.UnmarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (avatarRoot.TryGetComponent(out Vrm10Instance vrm10Instance))
            {
                Object.DestroyImmediate(vrm10Instance);
            }
        }
    }
}
