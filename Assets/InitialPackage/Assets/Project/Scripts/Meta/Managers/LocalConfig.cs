#region

using System;
using UnityEngine;

#endregion

namespace Project
{
    public static class LocalConfig
    {
        private class Keys
        {
            public const string UnlockedSkin = "UnlockedSkin{0}_{1}";
            public const string SkinClaimProgress = "SkinClaimProgress{0}_{1}";
            public const string SelectedSkin = "SelectedSkin_{0}";
            
            public const string IsSoundMuted = "IsMuted";
            public const string IsMusicMuted = "IsMusicMuted";
        }

        public static bool IsSoundMuted
        {
            get => GetBoolValue(Keys.IsSoundMuted, false);
            set => SetBoolValue(Keys.IsSoundMuted, value);
        }
        
        public static bool IsMusicMuted
        {
            get => GetBoolValue(Keys.IsMusicMuted, false);
            set => SetBoolValue(Keys.IsMusicMuted, value);
        }
        
        private static void SetBoolValue(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        private static bool GetBoolValue(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }
        
        private static DateTime GetDateTimeValue(string key, DateTime defaultValue)
        {
            DateTime time;
            string data = PlayerPrefs.GetString(key, "");
            if (String.IsNullOrEmpty(data))
                return defaultValue;

            if (data.TryDeserializeDateTime(out time))
                return time;

            return defaultValue;
        }

        private static void SetDateTimeValue(string key, DateTime value)
        {
            PlayerPrefs.SetString(key, value.Serialize());
        }
    }
}