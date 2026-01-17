using System;
using Ablet.API.V1.Querying;
using Ablet.Querying.Resolvers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Querying
{
    public partial class AQueryContext
    {
        public AQuery<Scene> GetLoadedScenes() => Query(new LoadedScenesQueryResolver());

        public AQuery<GameObject> GetRootGameObjects() => Query(new GetRootGameObjectsQueryResolver(GetLoadedScenes()));

        public AQuery<GameObject> FindGameObjects(string path) => Query(new FindRootGameObjectsQueryResolver(path));

        public AQuery<Component> GetSceneComponents(Type type, bool includeInactive = false)
        {
            return Query(new GetComponentsInChildrenQueryResolver(GetRootGameObjects(), type, includeInactive));
        }

        public AQuery<T> GetSceneComponents<T>(bool includeInactive = false)
        {
            return Query(new GetComponentsInChildrenQueryResolver<T>(GetRootGameObjects(), includeInactive));
        }
    }
}
