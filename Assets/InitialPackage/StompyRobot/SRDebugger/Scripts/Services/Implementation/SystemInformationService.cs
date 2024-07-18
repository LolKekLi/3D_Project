#region

using System;
using System.Collections.Generic;
using System.Linq;
using SRF;
using SRF.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace SRDebugger.Services.Implementation
{
    /// <summary>
    /// Reports system specifications and environment information. Information that can
    /// be used to identify a user is marked as private, and won't be included in any generated
    /// reports.
    /// </summary>
    [Service(typeof(ISystemInformationService))]
    public class StandardSystemInformationService : ISystemInformationService
    {
        private readonly Dictionary<string, IList<InfoEntry>> _info = new Dictionary<string, IList<InfoEntry>>();

        public StandardSystemInformationService()
        {
            CreateDefaultSet();
        }

        public IEnumerable<string> GetCategories()
        {
            return _info.Keys;
        }

        public IList<InfoEntry> GetInfo(string category)
        {
            IList<InfoEntry> list;

            if (!_info.TryGetValue(category, out list))
            {
                Debug.LogError("[SystemInformationService] Category not found: {0}".Fmt(category));
                return new InfoEntry[0];
            }

            return list;
        }

        public void Add(InfoEntry info, string category = "Default")
        {
            IList<InfoEntry> list;

            if (!_info.TryGetValue(category, out list))
            {
                list = new List<InfoEntry>();
                _info.Add(category, list);
            }

            if (list.Any(p => p.Title == info.Title))
            {
                throw new ArgumentException("An InfoEntry object with the same title already exists in that category.", "info");
            }

            list.Add(info);
        }

        public Dictionary<string, Dictionary<string, object>> CreateReport(bool includePrivate = false)
        {
            var dict = new Dictionary<string, Dictionary<string, object>>();

            foreach (var keyValuePair in _info)
            {
                var category = new Dictionary<string, object>();

                foreach (var systemInfo in keyValuePair.Value)
                {
                    if (systemInfo.IsPrivate && !includePrivate)
                    {
                        continue;
                    }

                    category.Add(systemInfo.Title, systemInfo.Value);
                }

                dict.Add(keyValuePair.Key, category);
            }

            return dict;
        }

        private void CreateDefaultSet()
        {
            _info.Add("System", new[]
            {
                InfoEntry.Create("Operating System", SystemInfo.operatingSystem),
                InfoEntry.Create("Device Name", SystemInfo.deviceName, true),
                InfoEntry.Create("Device Type", SystemInfo.deviceType),
                InfoEntry.Create("Device Model", SystemInfo.deviceModel),
                InfoEntry.Create("CPU Type", SystemInfo.processorType),
                InfoEntry.Create("CPU Count", SystemInfo.processorCount),
                InfoEntry.Create("System Memory", SRFileUtil.GetBytesReadable(((long) SystemInfo.systemMemorySize)*1024*1024))
                //Info.Create("Process Name", () => Process.GetCurrentProcess().ProcessName)
            });

            if (SystemInfo.batteryStatus != BatteryStatus.Unknown)
            {
                _info.Add("Battery", new[]
                {
                    InfoEntry.Create("Status", SystemInfo.batteryStatus),
                    InfoEntry.Create("Battery Level", SystemInfo.batteryLevel)
                });
            }

#if ENABLE_IL2CPP
            const string IL2CPP = "Yes";
#else
            const string IL2CPP = "No";
#endif

            _info.Add("Unity", new[]
            {
                InfoEntry.Create("Version", Application.unityVersion),
                InfoEntry.Create("Debug", Debug.isDebugBuild),
                InfoEntry.Create("Unity Pro", Application.HasProLicense()),
                InfoEntry.Create("Genuine",
                    "{0} ({1})".Fmt(Application.genuine ? "Yes" : "No",
                        Application.genuineCheckAvailable ? "Trusted" : "Untrusted")),
                InfoEntry.Create("System Language", Application.systemLanguage),
                InfoEntry.Create("Platform", Application.platform),
                InfoEntry.Create("Install Mode", Application.installMode),
                InfoEntry.Create("Sandbox", Application.sandboxType),
                InfoEntry.Create("IL2CPP", IL2CPP),
                InfoEntry.Create("Application Version", Application.version),
                InfoEntry.Create("SRDebugger Version", SRDebug.Version),
            });

            _info.Add("Display", new[]
            {
                InfoEntry.Create("Resolution", () => Screen.width + "x" + Screen.height),
                InfoEntry.Create("DPI", () => Screen.dpi),
                InfoEntry.Create("Fullscreen", () => Screen.fullScreen),
                InfoEntry.Create("Fullscreen Mode", () => Screen.fullScreenMode),
                InfoEntry.Create("Orientation", () => Screen.orientation),
            }); 

            _info.Add("Runtime", new[]
            {
                InfoEntry.Create("Play Time", () => Time.unscaledTime),
                InfoEntry.Create("Level Play Time", () => Time.timeSinceLevelLoad),
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                InfoEntry.Create("Current Level", () => Application.loadedLevelName),
#else
                InfoEntry.Create("Current Level", () =>
                {
                    var activeScene = SceneManager.GetActiveScene();
                    return "{0} (Index: {1})".Fmt(activeScene.name, activeScene.buildIndex);
                }),
#endif
                InfoEntry.Create("Quality Level",
                    () =>
                        QualitySettings.names[QualitySettings.GetQualityLevel()] + " (" +
                        QualitySettings.GetQualityLevel() + ")")
            });

            // Check for cloud build manifest
            var cloudBuildManifest = (TextAsset)Resources.Load("UnityCloudBuildManifest.json");
            var manifestDict = cloudBuildManifest != null
                ? Json.Deserialize(cloudBuildManifest.text) as Dictionary<string, object>
                : null;

            if (manifestDict != null)
            {
                var manifestList = new List<InfoEntry>(manifestDict.Count);

                foreach (var kvp in manifestDict)
                {
                    if (kvp.Value == null)
                    {
                        continue;
                    }

                    var value = kvp.Value.ToString();
                    manifestList.Add(InfoEntry.Create(GetCloudManifestPrettyName(kvp.Key), value));
                }

                _info.Add("Build", manifestList);
            }

            _info.Add("Features", new[]
            {
                InfoEntry.Create("Location", SystemInfo.supportsLocationService),
                InfoEntry.Create("Accelerometer", SystemInfo.supportsAccelerometer),
                InfoEntry.Create("Gyroscope", SystemInfo.supportsGyroscope),
                InfoEntry.Create("Vibration", SystemInfo.supportsVibration),
                InfoEntry.Create("Audio", SystemInfo.supportsAudio)
            });

#if UNITY_IOS

            _info.Add("iOS", new[] {

#if UNITY_5 || UNITY_5_3_OR_NEWER
                InfoEntry.Create("Generation", UnityEngine.iOS.Device.generation),
                InfoEntry.Create("Ad Tracking", UnityEngine.iOS.Device.advertisingTrackingEnabled),
#else
                InfoEntry.Create("Generation", iPhone.generation),
                InfoEntry.Create("Ad Tracking", iPhone.advertisingTrackingEnabled),
#endif
            });

#endif
#pragma warning disable 618
            _info.Add("Graphics - Device", new[]
            {
                InfoEntry.Create("Device Name", SystemInfo.graphicsDeviceName),
                InfoEntry.Create("Device Vendor", SystemInfo.graphicsDeviceVendor),
                InfoEntry.Create("Device Version", SystemInfo.graphicsDeviceVersion),
                InfoEntry.Create("Graphics Memory", SRFileUtil.GetBytesReadable(((long) SystemInfo.graphicsMemorySize)*1024*1024)),
                InfoEntry.Create("Max Tex Size", SystemInfo.maxTextureSize),
            });

            _info.Add("Graphics - Features", new[]
            {
                InfoEntry.Create("UV Starts at top", SystemInfo.graphicsUVStartsAtTop),
                InfoEntry.Create("Shader Level", SystemInfo.graphicsShaderLevel),
                InfoEntry.Create("Multi Threaded", SystemInfo.graphicsMultiThreaded),
                InfoEntry.Create("Hidden Service Removal (GPU)", SystemInfo.hasHiddenSurfaceRemovalOnGPU),
                InfoEntry.Create("Uniform Array Indexing (Fragment Shaders)", SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders),
                InfoEntry.Create("Shadows", SystemInfo.supportsShadows),
                InfoEntry.Create("Raw Depth Sampling (Shadows)", SystemInfo.supportsRawShadowDepthSampling),
                InfoEntry.Create("Motion Vectors", SystemInfo.supportsMotionVectors),
                InfoEntry.Create("Cubemaps", SystemInfo.supportsRenderToCubemap),
                InfoEntry.Create("Image Effects", SystemInfo.supportsImageEffects),
                InfoEntry.Create("3D Textures", SystemInfo.supports3DTextures),
                InfoEntry.Create("2D Array Textures", SystemInfo.supports2DArrayTextures),
                InfoEntry.Create("3D Render Textures", SystemInfo.supports3DRenderTextures),
                InfoEntry.Create("Cubemap Array Textures", SystemInfo.supportsCubemapArrayTextures),
                InfoEntry.Create("Copy Texture Support", SystemInfo.copyTextureSupport),
                InfoEntry.Create("Compute Shaders", SystemInfo.supportsComputeShaders),
                InfoEntry.Create("Instancing", SystemInfo.supportsInstancing),
                InfoEntry.Create("Hardware Quad Topology", SystemInfo.supportsHardwareQuadTopology),
                InfoEntry.Create("32-bit index buffer", SystemInfo.supports32bitsIndexBuffer),
                InfoEntry.Create("Sparse Textures", SystemInfo.supportsSparseTextures),
                InfoEntry.Create("Render Target Count", SystemInfo.supportedRenderTargetCount),
                InfoEntry.Create("Separated Render Targets Blend", SystemInfo.supportsSeparatedRenderTargetsBlend),
                InfoEntry.Create("Multisampled Textures", SystemInfo.supportsMultisampledTextures),
                InfoEntry.Create("Texture Wrap Mirror Once", SystemInfo.supportsTextureWrapMirrorOnce),
                InfoEntry.Create("Reversed Z Buffer", SystemInfo.usesReversedZBuffer)
            });
#pragma warning restore 618

        }

        private static string GetCloudManifestPrettyName(string name)
        {
            switch (name)
            {
                case "scmCommitId":
                    return "Commit";

                case "scmBranch":
                    return "Branch";

                case "cloudBuildTargetName":
                    return "Build Target";

                case "buildStartTime":
                    return "Build Date";
            }

            // Return name with first letter capitalised
            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }
    }
}
