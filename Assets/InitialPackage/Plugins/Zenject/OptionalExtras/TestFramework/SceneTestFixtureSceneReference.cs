#if UNITY_EDITOR

#region

using UnityEditor;
using UnityEngine;

#endregion

namespace Zenject
{
    public class SceneTestFixtureSceneReference : ScriptableObject
    {
        public SceneAsset Scene;
    }
}

#endif
