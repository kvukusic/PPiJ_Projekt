#region

using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Hoover.Settings
{
    /// <summary>
    /// This class contains all the phone applciation settings.
    /// </summary>
    public class ApplicationSettings
    {
        #region Singleton

        /// <summary>
        /// Private constructor.
        /// </summary>
        private ApplicationSettings()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ApplicationSettings()
        {
        }

        private static readonly ApplicationSettings _instance = new ApplicationSettings();
        public static ApplicationSettings Instance { get { return _instance; } }

        #endregion

        /// <summary>
        /// Holds the application settings.
        /// </summary>
        private IsolatedStorageSettings _settings;

        private const string ShowPreviewBoxKeyName = "ShowPreviewBox";
        private const bool ShowPreviewBoxDefault = true;
        public bool ShowPreviewBox
        {
            get { return LoadSettingLocal(ShowPreviewBoxKeyName, ShowPreviewBoxDefault); }
            set { if (StoreSettingLocal(ShowPreviewBoxKeyName, value)) StoreSettings(); }
        }

        /// <summary>
        /// Stores the settings to the isolated storage.
        /// </summary>
        private void StoreSettings()
        {
            ThreadPool.QueueUserWorkItem(func =>
            {
                //Save items using background thread.
                try
                {
                    //NOTE: may throw InvalidOperationException exception if user is in process of changing settings and navigating away from page
                    Thread.Sleep(200);
                    _settings.Save();
                }
                catch (Exception ex)
                {
                }
            });
        }

        /// <summary>
        /// Stores the given value by the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool StoreSettingLocal(string key, object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (_settings.Contains(key))
            {
                // If the value has changed
                if (_settings[key] != value)
                {
                    // Store the new value
                    _settings[key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key
            else
            {
                _settings.Add(key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        /// <summary>
        /// Loads the Application Settings with the given key if it exists or the default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T LoadSettingLocal<T>(string key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (_settings.Contains(key)) value = (T) _settings[key];
            // Otherwise use the default value.
            else value = defaultValue;

            return value;
        }

        /// <summary>
        /// Removes the Application Setting with the given key.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveSettingLocal(string key)
        {
            // If the key exists
            if (_settings.Contains(key))
            {
                _settings.Remove(key);
            }
        }

    }
}