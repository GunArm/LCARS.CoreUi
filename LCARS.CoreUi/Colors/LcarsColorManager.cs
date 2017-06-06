using LCARS.CoreUi.Helpers;
using Microsoft.VisualBasic;

namespace LCARS.CoreUi.Colors
{
    public class LcarsColorManager
    {
        private LcarsColorSet currentColorSet;
        private SettingsStore persistentSettings = new SettingsStore("LCARS");
        public LcarsColorManager()
        {
            ReloadColors();
        }

        public void ReloadColors()
        {
            string colorSettingCsv = persistentSettings.Load("Colors", "ColorMap");

            if (string.IsNullOrWhiteSpace(colorSettingCsv))
            {
                currentColorSet = LcarsColorSet.FromDefaults();
                return;
            }

            LcarsColorSet holder;
            try
            {
                holder = LcarsColorSet.FromCsv(colorSettingCsv);
            }
            catch
            {
                holder = LcarsColorSet.FromDefaults();
            }
            currentColorSet = holder;
        }

        private void SetDefaultColors()
        {
            currentColorSet = LcarsColorSet.FromDefaults();
            Interaction.SaveSetting("LCARS x32", "Colors", "ColorMap", currentColorSet.ToCsv());
        }
    }
}
