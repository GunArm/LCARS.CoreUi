﻿using System.IO;
using System.Reflection;

namespace LCARS.CoreUi.Helpers
{
    public static class Paths
    {
        public static string AppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string FontsDir = Path.Combine(AppDir, "Assets/Fonts");
        public static string SoundsDir = Path.Combine(AppDir, "Assets/Sounds");
    }
}
