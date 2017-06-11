using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LCARS.CoreUi.Colors
{
    public class LcarsColorSet
    {
        private Dictionary<LcarsColorFunction, string> colorSet = new Dictionary<LcarsColorFunction, string>();

        private LcarsColorSet()
        {
            // nothing should call this
            throw new NotImplementedException();
        }

        private LcarsColorSet(string[] colorStringArray)
        {
            for (int i = 0; i < colorStringArray.Length; i++)
            {
                colorSet[(LcarsColorFunction)i] = colorStringArray[i];
            }
        }

        public static LcarsColorSet FromStringArray(string[] colorStringArray)
        {
            if (colorStringArray.Length != Enum<LcarsColorFunction>.Count)
            {
                throw new Exception("Color string array somehow the wrong length");
            }
            return new LcarsColorSet(colorStringArray);
        }

        public static LcarsColorSet FromCsv(string colorStringCsv)
        {
            if (string.IsNullOrWhiteSpace(colorStringCsv))
            {
                throw new Exception("Color setting string not initialized");
            }

            var split = colorStringCsv.Split(',');
            if (split.Length != Enum<LcarsColorFunction>.Count)
            {
                throw new Exception("Color setting string somehow the wrong split length");
            }
            return FromStringArray(split);
        }

        public static LcarsColorSet FromDefaults()
        {
            return FromStringArray(new string[]
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
             });
        }

        public void SetFunctionColor(LcarsColorFunction colorFunction, string color)
        {
            // warning: directly calling an LcarsColorManager.CurrentColorSet.SetFunctionColor(.....) will not trigger redraw
            colorSet[colorFunction] = color;
        }

        public Color GetFunctionNativeColor(LcarsColorFunction colorFunction)
        {
            if (!Enum.IsDefined(typeof(LcarsColorFunction), colorFunction))
            {
                throw new Exception("Invalid LcarsColorStyle specified");
            }
            return ColorTranslator.FromHtml(colorSet[colorFunction]);
        }

        public string ToCsv()
        {
            var stringArray = new string[Enum<LcarsColorFunction>.Count];
            for (int i = 0; i < stringArray.Length; i++) stringArray[i] = colorSet[(LcarsColorFunction)i];
            return string.Join(",", stringArray);
        }
    }
}
