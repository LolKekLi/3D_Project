#region

using SRDebugger.Internal;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace SRDebugger.UI.Other
{
    [RequireComponent(typeof (ScrollRect))]
    public class ScrollSettingsBehaviour : MonoBehaviour
    {
        public const float ScrollSensitivity = 40f;

        private void Awake()
        {
            var scrollRect = GetComponent<ScrollRect>();
            scrollRect.scrollSensitivity = ScrollSensitivity;

            if (!SRDebuggerUtil.IsMobilePlatform)
            {
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
                scrollRect.inertia = false;
            }
        }
    }
}
