#region

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

#endregion

namespace Project
{
    public static class DebugSafe
    {
        [Conditional("FORCE_DEBUG")]
        public static void Log(object message) => Debug.unityLogger.Log(LogType.Log, message);
        
        [Conditional("FORCE_DEBUG")]
        public static void LogError(object message) => Debug.unityLogger.Log(LogType.Error, message);
        
        [Conditional("FORCE_DEBUG")]
        public static void LogException(Exception exception) => Debug.unityLogger.LogException(exception, (Object) null);
        
        [Conditional("FORCE_DEBUG")]
        public static void LogWarning(object message) => Debug.unityLogger.Log(LogType.Warning, message);
    }
}