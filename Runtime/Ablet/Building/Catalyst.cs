using Ablet.API.V1;

namespace Ablet.Building
{
    class Catalyst
    {
        public readonly string Id;
        public AssetGenerationMode WillCloneSceneObject;
        public AssetGenerationMode WillPersistGeneratedAssets;
        public bool IsPartial;
        public BuildInitiationSourceMode BuildInitiationSourceMode;
        public ObjectRetainMode ObjectRetainMode;

        public PreviewMode PreviewMode;
        public bool IsObservable;

        public Catalyst(string id)
        {
            Id = id;
        }
    }
}
