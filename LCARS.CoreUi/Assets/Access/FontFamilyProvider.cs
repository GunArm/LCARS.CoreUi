using LCARS.CoreUi.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace LCARS.CoreUi.Assets.Access
{
    public static class FontFamilyProvider
    {
        private static Dictionary<string, FontFamily> lcarsFontFamilies;
        private static Dictionary<string, FontFamily> alienFontFamilies;
        private static Dictionary<string, FontFamily> miscFontFamilies;

        public static FontFamily LcarsLight { get { return lcarsFontFamilies["lcars-lite"]; } }
        public static FontFamily LcarsHeavy { get { return lcarsFontFamilies["lcars-full"]; } }
        public static FontFamily RandomAlien { get { return alienFontFamilies.ElementAt(Randomizer.NextInt(0, alienFontFamilies.Count)).Value; } }

        static FontFamilyProvider()
        {
            lcarsFontFamilies = GetFontCollection("LCARS");
            alienFontFamilies = GetFontCollection("Alien");
            miscFontFamilies = GetFontCollection("Misc");
        }

        private static Dictionary<string, FontFamily> GetFontCollection(string fontGroup)
        {
            string subFolderPath = Path.Combine(Paths.FontsDir, fontGroup);

            var fonts = new Dictionary<string, FontFamily>(StringComparer.InvariantCultureIgnoreCase);
            if (!Directory.Exists(subFolderPath)) return fonts;

            foreach (var fontFilePath in Directory.EnumerateFiles(subFolderPath))
            {
                if (!fontFilePath.ToLower().EndsWith("ttf") && !fontFilePath.ToLower().EndsWith("otf")) continue;

                var families = ReadFontFile(fontFilePath);
                if (families.Length != 1) throw new Exception("Font file " + fontFilePath + " has more (or less) than one font");
                fonts.Add(Path.GetFileNameWithoutExtension(fontFilePath), families[0]);
            }

            return fonts;
        }

        private static FontFamily[] ReadFontFile(string fontFileName)
        {
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(fontFileName);
            return pfc.Families;
        }
    }
}