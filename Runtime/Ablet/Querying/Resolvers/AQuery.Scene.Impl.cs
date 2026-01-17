using System.Collections.Generic;
using Ablet.API.V1.Querying;
using Ablet.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Querying.Resolvers
{
    class LoadedScenesQueryResolver : AQueryResolverBase<Scene>
    {
        protected override IEnumerable<Scene> ResolveImpl()
        {
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).isLoaded)
                {
                    yield return SceneManager.GetSceneAt(i);
                }
            }
        }
    }

    class GetRootGameObjectsQueryResolver : ASelectQueryResolverBase<Scene, GameObject>
    {
        public GetRootGameObjectsQueryResolver(AQuery<Scene> query) : base(query) { }

        protected override IEnumerable<GameObject> Selector(Scene item) => item.GetRootGameObjects();
    }
    
    class FindRootGameObjectsQueryResolver : AQueryResolverBase<GameObject>
    {
        readonly string _path;
        public FindRootGameObjectsQueryResolver(string path) => _path = path;

        protected override IEnumerable<GameObject> ResolveImpl()
        {
            if (AbletRuntimeUtil.FromAbsolutePath(_path) is { } transform)
            {
                yield return transform.gameObject;
            }
        }
    }

    class FindSceneGameObjectsQueryResolver : ASelectQueryResolverBase<Scene, GameObject>
    {
        readonly string _path;
        public FindSceneGameObjectsQueryResolver(AQuery<Scene> query, string path) : base(query) => _path = path;

        protected override IEnumerable<GameObject> Selector(Scene item)
        {
            if (AbletRuntimeUtil.FromAbsolutePath(_path, item) is { } transform)
            {
                yield return transform.gameObject;
            }
        }
    }
}
