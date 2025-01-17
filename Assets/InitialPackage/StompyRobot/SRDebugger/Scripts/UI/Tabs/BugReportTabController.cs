﻿#region

using SRDebugger.UI.Other;
using SRF;
using UnityEngine;

#endregion

namespace SRDebugger.UI.Tabs
{
    public class BugReportTabController : SRMonoBehaviourEx, IEnableTab
    {
        [RequiredField] public BugReportSheetController BugReportSheetPrefab;

        [RequiredField] public RectTransform Container;

        public bool IsEnabled
        {
            get { return Settings.Instance.EnableBugReporter; }
        }

        protected override void Start()
        {
            base.Start();

            var sheet = SRInstantiate.Instantiate(BugReportSheetPrefab);
            sheet.IsCancelButtonEnabled = false;

            // Callbacks when taking screenshot will hide the debug panel so it is not present in the image
            sheet.TakingScreenshot = TakingScreenshot;
            sheet.ScreenshotComplete = ScreenshotComplete;

            sheet.CachedTransform.SetParent(Container, false);
        }

        private void TakingScreenshot()
        {
            SRDebug.Instance.HideDebugPanel();
        }

        private void ScreenshotComplete()
        {
            SRDebug.Instance.ShowDebugPanel(false);
        }
    }
}
