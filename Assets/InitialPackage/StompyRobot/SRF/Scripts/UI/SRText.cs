﻿#region

using System;
using SRF.Internal;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace SRF.UI
{
    /// <summary>
    /// Adds a LayoutDirty callback to the default Text component.
    /// </summary>
    [AddComponentMenu(ComponentMenuPaths.SRText)]
    public class SRText : Text
    {
        public event Action<SRText> LayoutDirty;

        public override void SetLayoutDirty()
        {
            base.SetLayoutDirty();

            if (LayoutDirty != null)
            {
                LayoutDirty(this);
            }
        }
    }
}
