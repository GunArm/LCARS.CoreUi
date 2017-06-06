using LCARS.CoreUi.Enums;
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
            int length = Enum<LcarsColorFunction>.Count;
            var stringArray = new string[length];
            for (int i = 0; i < length; i++) stringArray[i] = colorSet[(LcarsColorFunction)i];
            return string.Join(",", stringArray);
        }
    }
}
