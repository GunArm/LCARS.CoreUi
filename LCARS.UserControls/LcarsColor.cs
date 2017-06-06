using LCARS.UserControls.Enums;
using Microsoft.VisualBasic;
using System;
using System.Drawing;

namespace LCARS.UserControls
{
    public class LcarsColor
    {
        private string[] currentColorSet;
        public string[] CurrentColorSet
        {
            get { return currentColorSet; }
            set
            {
                for (int i = 0; i <= currentColorSet.GetUpperBound(0) && i <= value.GetUpperBound(0); i++)
                {
                    currentColorSet[i] = value[i];
                }
            }
        }
        public LcarsColor()
        {
            ReloadColors();
        }

        public void ReloadColors()
        {
            string colorSettingString = Interaction.GetSetting("LCARS x32", "Colors", "ColorMap", "NONE");

            if (string.IsNullOrWhiteSpace(colorSettingString) || colorSettingString == "NONE")
            {
                SetDefaultColors();
                return;
            }

            var split = colorSettingString.Split(',');
            if (split.Length != Enum.GetValues(typeof(LcarsColorStyles)).Length)
            {
                SetDefaultColors();
                return;
            }
            currentColorSet = split;
        }

        private void SetDefaultColors()
        {
            string[] DefaultColors =
            {
                    "#3366CC",
                    "#99CCFF",
                    "#CC99CC",
                    "#FFCC00",
                    "#FFFF99",
                    "#CC6666",
                    "#FFFFFF",
                    "#FF0000",
                    "#FFCC66",
                    "Orange",
                    "#99CCFF"
                };
            currentColorSet = DefaultColors;
            Interaction.SaveSetting("LCARS x32", "Colors", "ColorMap", Strings.Join(currentColorSet, ","));
        }

        public LcarsColorStyles? IndexOf(string Name)
        {
            foreach (LcarsColorStyles style in Enum.GetValues(typeof(LcarsColorStyles)))
            {
                if (style.ToString().ToLower() == Name.ToLower())
                {
                    return style;
                }
            }
            return null;
        }

        public Color getColor(LcarsColorStyles colorStyle)
        {
            if (!Enum.IsDefined(typeof(LcarsColorStyles), colorStyle))
            {
                throw new Exception("Invalid LcarsColorStyle used");
            }
            return ColorTranslator.FromHtml(currentColorSet[(int)colorStyle]);
        }
    }
}
