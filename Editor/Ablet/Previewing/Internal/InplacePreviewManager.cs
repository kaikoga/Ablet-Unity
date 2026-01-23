using Ablet.API.V1;
using Ablet.API.V1.Attributes;
using Ablet.Building;
using Ablet.Builtin.Layers;
using Ablet.Builtin.Layers.Avatar;
using Ablet.Models;
using Ablet.Planning;
using Ablet.Previewing.Internal.Presentation;
using Ablet.Registries;
using Ablet.Repositories;
using UnityEditor;
using UnityEngine;

namespace Ablet.Previewing.Internal
{
    class InplacePreviewManager
    {
        public static readonly InplacePreviewManager Instance = new InplacePreviewManager();

        InplacePreviewRequest? _currentPreviewRequest;
        bool _isRefreshing;

        public bool IsBlocked(InplacePreviewRequest request) => _currentPreviewRequest != null && _currentPreviewRequest != request;

        [InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
        }

        InplacePreviewManager()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
            Selection.selectionChanged += TryRefreshCurrentPreviewLater;
            EditorStateRepository.Instance.OnChanged += TryRefreshCurrentPreviewLater;
#if UNITY_2022_3_OR_NEWER
            ObjectChangeEvents.changesPublished += OnChangesPublished;
#endif
        }

#if UNITY_2022_3_OR_NEWER
        void OnChangesPublished(ref ObjectChangeEventStream stream)
        {
            if (stream.length > 0)
            {
                TryRefreshCurrentPreviewLater();
            }
        }
#endif

        public void Request(InplacePreviewRequest previewRequest)
        {
            if (_currentPreviewRequest != null)
            {
                if (previewRequest != _currentPreviewRequest)
                {
                    // blocked
                    return;
                }
            }
            _currentPreviewRequest = previewRequest;
            TryRefreshCurrentPreviewLater();
        }

        public void DisposeRequest(InplacePreviewRequest previewRequest)
        {
            if (_currentPreviewRequest == previewRequest)
            {
                _currentPreviewRequest = null;
            }
            TryRefreshCurrentPreviewLater();
        }

        void TryRefreshCurrentPreviewLater()
        {
            if (!_isRefreshing)
            {
                _isRefreshing = true;
                EditorApplication.delayCall += TryRefreshCurrentPreview;
            }
        }

        void TryRefreshCurrentPreview()
        {
            _isRefreshing = false;
            InplacePreviewPresenter.Instance.EndPreview();

            GameObject? originalAvatar = null;
            AbletPlatform? platform = null;

            if (_currentPreviewRequest?.OriginalAvatar is { } requestedAvatar)
            {
                // preview request with / without enhanced preview
                originalAvatar = requestedAvatar;
                PlatformRegistry.Instance.TryGuessPlatform(originalAvatar, out platform);
            }
            else if (EditorStateRepository.Instance.IsEnhancedInplacePreview)
            {
                // try enhanced preview without preview request 
                PlatformRegistry.Instance.TryGuessEntrypointObject(Selection.activeGameObject, out originalAvatar, out platform);
            }

            if (originalAvatar != null && platform != null)
            {
                var avatar = InplacePreviewPresenter.Instance.StartPreview(originalAvatar);
                if (EditorStateRepository.Instance.IsEnhancedInplacePreview)
                {
                    ApplyEnhancedPreview(avatar, platform, _currentPreviewRequest?.Posing);
                }
                else
                {
                    ApplyPosingOnly(avatar, platform, _currentPreviewRequest?.Posing);
                }
            }
        }

        static void ApplyEnhancedPreview(GameObject avatar, AbletPlatform platform, AbletObservableProcedure? posingProcedure)
        {
            var arguments = BuildArgument.FromInplacePreview(avatar, platform)
                .AddInput(new InplacePreviewPosingInput(posingProcedure));
            AbletFacade.BuildWithArguments(arguments);
        }

        static void ApplyPosingOnly(GameObject avatar, AbletPlatform platform, AbletObservableProcedure? posingProcedure)
        {
            var arguments = BuildArgument.FromInplacePreview(avatar, platform)
                .AddInput(new InplacePreviewPosingInput(posingProcedure));
            var layer = LayerRegistry.Instance.Get<InplacePreviewPosingLayer>();
            var process = BuildProcess.FromSinglePass(new AbletPass(layer), arguments);
            process.Build();
        }

        void Dispose()
        {
            _currentPreviewRequest = null;
            InplacePreviewPresenter.Instance.EndPreview();
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
            EditorStateRepository.Instance.OnChanged -= TryRefreshCurrentPreview;
#if UNITY_2022_3_OR_NEWER
            ObjectChangeEvents.changesPublished -= OnChangesPublished;
#endif
        }
    }

}
