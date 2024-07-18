#if !NOT_UNITY3D

#region

using ModestTree;
using UnityEngine;

#endregion

namespace Zenject
{
    [NoReflectionBaking]
    public class PrefabProvider : IPrefabProvider
    {
        readonly Object _prefab;

        public PrefabProvider(Object prefab)
        {
            Assert.IsNotNull(prefab);
            _prefab = prefab;
        }

        public Object GetPrefab(InjectContext _)
        {
            return _prefab;
        }
    }
}

#endif


