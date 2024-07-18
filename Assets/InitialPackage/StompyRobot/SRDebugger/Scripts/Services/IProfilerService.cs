
#region

using System;
using SRDebugger.Profiler;
using SRF.Service;

#endregion

namespace SRDebugger.Services
{
    #region

#if UNITY_2018_1_OR_NEWER
    using UnityEngine.Rendering;
#endif

    #endregion

    public static class ProfilerServiceSelector
    {
        [ServiceSelector(typeof(IProfilerService))]
        public static Type GetProfilerServiceType()
        {
#if UNITY_2018_1_OR_NEWER
            if(GraphicsSettings.renderPipelineAsset != null)
            {
                return typeof(SRPProfilerService);
            }
#endif

            return typeof(ProfilerServiceImpl);
        }
    }

    public struct ProfilerFrame
    {
        public double FrameTime;
        public double OtherTime;
        public double RenderTime;
        public double UpdateTime;
    }

    public interface IProfilerService
    {
        float AverageFrameTime { get; }
        float LastFrameTime { get; }
        CircularBuffer<ProfilerFrame> FrameBuffer { get; }
    }
}
