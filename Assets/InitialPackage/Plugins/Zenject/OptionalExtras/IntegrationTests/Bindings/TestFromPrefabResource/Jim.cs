#region

using System;
using UnityEngine;

#endregion

namespace Zenject.Tests.Bindings.FromPrefabResource
{
    public class Jim : MonoBehaviour
    {
        [NonSerialized]
        [Inject]
        public Bob Bob;
    }
}
