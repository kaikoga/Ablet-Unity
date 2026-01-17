using Ablet.API.V1.Querying;
using Ablet.Querying.Resolvers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Querying
{
    public static partial class AQuery
    {
        public static AQuery<GameObject> GetRootGameObjects(this AQuery<Scene> query) => query.Create(new GetRootGameObjectsQueryResolver(query));

        public static AQuery<GameObject> FindGameObjects(this AQuery<Scene> query, string path) => query.Create(new FindSceneGameObjectsQueryResolver(query, path));

    }
}
