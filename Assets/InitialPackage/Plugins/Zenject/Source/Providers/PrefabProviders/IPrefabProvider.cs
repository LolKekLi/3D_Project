#region

using UnityEngine;

#endregion

#if !NOT_UNITY3D

namespace Zenject
{
    public interface IPrefabProvider
    {
        Object GetPrefab(InjectContext context);
    }
}

#endif

