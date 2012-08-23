using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Fotoideen.Helper
{
    public class ApplicationSettings
    {
        public static string GetSetting(string key)
        {
            var settingsStorage = IsolatedStorageSettings.ApplicationSettings;
            if (settingsStorage.Contains(key))
            {
                return settingsStorage[key] as string;
            }
            return string.Empty;
        }

        public static T GetSetting<T>(string key)
        {
            var settingsStorage = IsolatedStorageSettings.ApplicationSettings;
            if (settingsStorage.Contains(key))
            {
                return (T)settingsStorage[key];
            }
            return default(T);
        }

        public static void SaveSetting(string key, string value)
        {
            var settingsStorage = IsolatedStorageSettings.ApplicationSettings;
            if(settingsStorage.Contains(key))
            {
                settingsStorage[key] = value;
            }
            else
            {
                settingsStorage.Add(key, value);
            }
            settingsStorage.Save();
        }

        public static void SaveSetting<T>(string key, T value)
        {
            var settingsStorage = IsolatedStorageSettings.ApplicationSettings;
            if (settingsStorage.Contains(key))
            {
                settingsStorage[key] = value;
            }
            else
            {
                settingsStorage.Add(key, value);
            }
            settingsStorage.Save();
        }

        public static void ClearSettings()
        {
            var settingsStorage = IsolatedStorageSettings.ApplicationSettings;
            settingsStorage.Clear();
            settingsStorage.Save();
        }
    }
}
