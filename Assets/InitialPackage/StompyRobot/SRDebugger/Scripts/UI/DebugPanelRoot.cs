﻿#region

using SRDebugger.Scripts;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using UnityEngine;

#endregion

namespace SRDebugger.UI
{
    public class DebugPanelRoot : SRMonoBehaviourEx
    {
        [RequiredField] public Canvas Canvas;

        [RequiredField] public CanvasGroup CanvasGroup;

        [RequiredField] public DebuggerTabController TabController;

        public void Close()
        {
            if (Settings.Instance.UnloadOnClose)
            {
                SRServiceManager.GetService<IDebugService>().DestroyDebugPanel();
            }
            else
            {
                SRServiceManager.GetService<IDebugService>().HideDebugPanel();
            }
        }

        public void CloseAndDestroy()
        {
            SRServiceManager.GetService<IDebugService>().DestroyDebugPanel();
        }
    }
}
