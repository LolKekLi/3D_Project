#region

using UnityEngine;

#endregion

namespace Zenject.Tests.AutoInjecter
{
    public class Gorp : MonoBehaviour
    {
        [Inject]
        public DiContainer Container;
    }
}

