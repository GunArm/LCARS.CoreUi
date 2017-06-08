using System.IO;
using System.Reflection;

namespace LCARS.CoreUi
{
    public static class Paths
    {
        public static string AppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string FontsDir = Path.Combine(AppDir, "Assets/Fonts");
    }
}
