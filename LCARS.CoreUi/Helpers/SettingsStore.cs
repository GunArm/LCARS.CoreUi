using Microsoft.VisualBasic;

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
    }
}
