using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace LCARS.CoreUi
{
    public static class FontProvider
    {
        public static Dictionary<string, FontFamily> LcarsFonts { get; private set; }
        public static Dictionary<string, FontFamily> AlienFonts { get; private set; }
        public static Dictionary<string, FontFamily> MiscFonts { get; private set; }

        static FontProvider()
        {
            LcarsFonts = GetFontCollection("LCARS");
            AlienFonts = GetFontCollection("Alien");
            MiscFonts = GetFontCollection("Misc");
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
                fonts.Add(fontFilePath, families[0]);
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