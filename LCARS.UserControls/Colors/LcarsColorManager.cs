using Microsoft.VisualBasic;

namespace LCARS.UserControls.Colors
{
    public class LcarsColorManager
    {
        private LcarsColorSet currentColorSet;
        public LcarsColorManager()
        {
            ReloadColors();
        }

        public void ReloadColors()
        {
            string colorSettingCsv = Interaction.GetSetting("LCARS x32", "Colors", "ColorMap", "NONE");

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
