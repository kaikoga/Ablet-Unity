using System;
using Ablet.API;
using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.API.V1.Extensions.Platform;
using Ablet.Utils;
using UnityEngine;
using VRM;
using Object = UnityEngine.Object;

namespace Ablet.Builtin.UniVRM
{
    [AbletPlatform]
    public class UniVRMPlatform : IAbletPlatform
    {
        string IAbletDefinition.Id => BuiltinPlatformIds.UniVRM;
        string IAbletDefinition.DisplayName => "UniVRM VRM0";
        int IAbletPlatform.Priority => 0;

        Type IAbletPlatform.EntrypointComponentType => typeof(VRMMeta);
        bool IAbletPlatform.FilterEntrypoint(Component component) => true;
    }
    
    [AbletExtension]
    class UniVRMExtension : IApplyOnPlaySupportExtension, IConvertiblePlatformExtension
    {
        Type IAbletExtension.ForType => typeof(UniVRMPlatform);
        
        void IConvertiblePlatformExtension.MarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (!avatarRoot.TryGetComponent(out VRMMeta vrmMeta))
            {
                vrmMeta = avatarRoot.AddComponent<VRMMeta>();
            }

            // Always provide data structure
            if (!vrmMeta.Meta)
            {
                var meta = ScriptableObject.CreateInstance<VRMMetaObject>();
                meta.Title = AbletRuntimeUtil.GuessOriginalAvatarName(avatarRoot);
                meta.Author = AbletRuntimeUtil.VrmAuthor;
                meta.Version = AbletRuntimeUtil.VrmVersion;
                vrmMeta.Meta = meta;
            }
            if (!avatarRoot.TryGetComponent(out VRMBlendShapeProxy blendShapeProxy))
            {
                blendShapeProxy = avatarRoot.AddComponent<VRMBlendShapeProxy>();
            }
            if (!blendShapeProxy.BlendShapeAvatar)
            {
                blendShapeProxy.BlendShapeAvatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
            }
            if (!avatarRoot.TryGetComponent(out VRMFirstPerson _))
            {
                avatarRoot.AddComponent<VRMFirstPerson>().SetDefault();
            }
        }

        void IConvertiblePlatformExtension.MaterializeAsAvatarRoot(GameObject avatarRoot)
        {
            // do nothing
        }

        void IConvertiblePlatformExtension.UnmarkAsAvatarRoot(GameObject avatarRoot)
        {
            if (avatarRoot.TryGetComponent(out VRMMeta vrmMeta))
            {
                Object.DestroyImmediate(vrmMeta);
            }
        }
    }
}
