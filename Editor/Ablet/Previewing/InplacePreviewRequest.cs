using System;
using Ablet.API.V1;
using Ablet.Previewing.Internal;
using UnityEngine;

namespace Ablet.Previewing
{
    public class InplacePreviewRequest : IDisposable
    {
        public readonly GameObject OriginalAvatar;

        AbletObservableProcedure? _posing;
        public AbletObservableProcedure? Posing
        {
            get => _posing;
            set
            {
                _posing = value;
                InplacePreviewManager.Instance.Request(this);

            }
        }

        public bool IsBlocked => InplacePreviewManager.Instance.IsBlocked(this);

        public InplacePreviewRequest(GameObject originalAvatar)
        {
            OriginalAvatar = originalAvatar;
        }

        public void Dispose()
        {
            InplacePreviewManager.Instance.DisposeRequest(this);
        }
    }
}
