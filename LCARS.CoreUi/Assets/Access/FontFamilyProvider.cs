using LCARS.CoreUi.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LCARS.CoreUi.Assets.Access
{
    public static class FontFamilyProvider
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();

        public static Dictionary<string, FontFamily> LcarsFontFamilies { get; private set; }
        public static Dictionary<string, FontFamily> AlienFontFamilies { get; private set; }
        public static Dictionary<string, FontFamily> MiscFontFamilies { get; private set; }

        public static FontFamily LcarsLight { get { return LcarsFontFamilies["lcars-lite"]; } }
        public static FontFamily LcarsHeavy { get { return LcarsFontFamilies["lcars-full"]; } }
        public static FontFamily RandomAlien { get { return AlienFontFamilies.ElementAt(Randomizer.NextInt(0, AlienFontFamilies.Count)).Value; } }

        static FontFamilyProvider()
        {
            LcarsFontFamilies = LoadFontFamiliesFromResources(new List<string>
            {
                "LCARS.CoreUi.Assets.Fonts.LCARS.lcars-full.ttf",
                "LCARS.CoreUi.Assets.Fonts.LCARS.lcars-lite.ttf",
            });

            AlienFontFamilies = LoadFontFamiliesFromResources(new List<string>
            {
                "LCARS.CoreUi.Assets.Fonts.Alien.bajoran.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.cardassian.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.dominion.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.fabrini.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.ferengi.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.klingon.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.romulan.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.tholian.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.trill.ttf",
                "LCARS.CoreUi.Assets.Fonts.Alien.vulcan.ttf",
            });

            MiscFontFamilies = LoadFontFamiliesFromResources(new List<string>
            {
                "LCARS.CoreUi.Assets.Fonts.Misc.tng_credits.ttf",
                "LCARS.CoreUi.Assets.Fonts.Misc.tng_monitors.ttf",
                "LCARS.CoreUi.Assets.Fonts.Misc.tng_title.ttf",
                "LCARS.CoreUi.Assets.Fonts.Misc.trekbats.ttf",
            });
        }

        private static Dictionary<string, FontFamily> LoadFontFamiliesFromResources(List<string> namespacePaths)
        {
            var result = new Dictionary<string, FontFamily>();
            foreach (string namespacePath in namespacePaths)
            {
                var bits = namespacePath.Split('.');
                string extension = bits[bits.Length - 1];
                if (!extension.ToLower().EndsWith("ttf") && !extension.ToLower().EndsWith("otf")) throw new Exception("This error message sucks");

                var families = GetFontFamiliesFromResource(namespacePath);
                if (families.Length != 1) throw new Exception("Font file " + namespacePath + " has more (or less) than one font");
                string key = bits[bits.Length - 2];
                result.Add(key, families[0]);
            }
            return result;
        }

        private static FontFamily[] GetFontFamiliesFromResource(string fontNamespacePath)
        {
            var pfc = new PrivateFontCollection();
            Stream fontStream = assembly.GetManifestResourceStream(fontNamespacePath);

            byte[] fontdata = new byte[fontStream.Length];
            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            fontStream.Close();
            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    pfc.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
                }
            }
            return pfc.Families;
        }
    }
}
