#region

using System.Collections;
using UnityEngine;

#endregion

namespace SRF
{
    public static class Coroutines
    {
        public static IEnumerator WaitForSecondsRealTime(float time)
        {
            var endTime = Time.realtimeSinceStartup + time;

            while (Time.realtimeSinceStartup < endTime)
            {
                yield return null;
            }
        }
    }
}
