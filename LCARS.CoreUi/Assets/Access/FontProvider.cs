using System.Drawing;

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

        public static Font RandomAlien(float size, GraphicsUnit graphicsUnit = GraphicsUnit.Point)
        {
            return new Font(FontFamilyProvider.RandomAlien, size, FontStyle.Regular, graphicsUnit);
        }
    }
}
