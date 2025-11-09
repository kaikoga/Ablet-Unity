using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ablet.Querying
{
    public class LoadedScenesQuery : AMultipleQuery<Scene>
    {
        public override IEnumerable<Scene> Query()
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

    public class GetRootGameObjectsQuery : AMultipleQuery<GameObject>
    {
        public override IEnumerable<GameObject> Query()
        {
            return new LoadedScenesQuery().Query()
                .SelectMany(scene => scene.GetRootGameObjects());
        }
    }

    public class GetComponentsQuery : AMultipleQuery<Component>
    {
        readonly Type _type;
        readonly bool _includeInactive;

        public GetComponentsQuery(Type type, bool includeInactive = false)
        {
            _type = type;
            _includeInactive = includeInactive;            
        }

        public override IEnumerable<Component> Query()
        {
            return new GetRootGameObjectsQuery().Query()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren(_type, _includeInactive));
        }
    }

    public class GetComponentsQuery<T> : AMultipleQuery<T>
    {
        readonly bool _includeInactive;

        public GetComponentsQuery(bool includeInactive = false)
        {
            _includeInactive = includeInactive;            
        }

        public override IEnumerable<T> Query()
        {
            return new GetRootGameObjectsQuery().Query()
                .SelectMany(gameObject => gameObject.GetComponentsInChildren<T>(_includeInactive));
        }
    }
}
