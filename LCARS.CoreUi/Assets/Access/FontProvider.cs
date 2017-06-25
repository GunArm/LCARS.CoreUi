using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LCARS.CoreUi.Assets.Access
{
    public static class FontProvider
    {
        public static Font Lcars(float size, GraphicsUnit graphicsUnit = GraphicsUnit.Point)
        {
            return new Font(FontFamilyProvider.LcarsLight, size, FontStyle.Regular, graphicsUnit);
        }

        public static Font Lcars(float size, FontStyle fontStyle, GraphicsUnit graphicsUnit = GraphicsUnit.Point)
        {
            return new Font(FontFamilyProvider.LcarsLight, size, fontStyle, graphicsUnit);
        }

        public static List<string> AlienList { get { return FontFamilyProvider.AlienFontFamilies.Keys.ToList(); } }

        public static Font Alien(string species, float size, GraphicsUnit graphicsUnit = GraphicsUnit.Point)
        {
            FontFamily test = FontFamilyProvider.AlienFontFamilies[species];
            return new Font(test, size, FontStyle.Regular, graphicsUnit);
        }

        public static Font RandomAlien(float size, GraphicsUnit graphicsUnit = GraphicsUnit.Point)
        {
            return new Font(FontFamilyProvider.RandomAlien, size, FontStyle.Regular, graphicsUnit);
        }
    }
}
