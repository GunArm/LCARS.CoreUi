using Microsoft.VisualBasic;
using System;

namespace LCARS.CoreUi.Helpers
{
    public class SettingsStore
    {
        public string AppName { get; set; }

        public SettingsStore(string appName)
        {
            AppName = appName;
        }

        public void Save(string section, string key, string value)
        {
            Interaction.SaveSetting(AppName, section, key, value);
        }

        public string Load(string section, string key, string defaultValue = null)
        {
            return Interaction.GetSetting(AppName, section, key, defaultValue);
        }

        /// <summary>
        /// Equivalent to DeleteSetting, but will not throw ArgumentException
        /// </summary>
        /// <returns>True if setting existed to be deleted</returns>
        public bool TryDelete(string section = null, string key = null)
        {
            try
            {
                Interaction.DeleteSetting(AppName, section, key);
                return true;
            }
            catch (ArgumentException ex)
            {
                return false;
            }
        }
    }
}
