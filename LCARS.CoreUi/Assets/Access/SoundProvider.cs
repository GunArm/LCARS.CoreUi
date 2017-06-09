using LCARS.CoreUi.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LCARS.CoreUi.Assets.Access
{
    public static class SoundProvider
    {
        private static Dictionary<string, string> sounds;

        public static string PlainBeep { get { return sounds["207"]; } }
        public static string RandomShortBeep { get { return sounds.ElementAt(Randomizer.NextInt(0, sounds.Count)).Value; } }

        static SoundProvider()
        {
            sounds = GetSoundCollection("ShortBeeps");
        }

        private static Dictionary<string, string> GetSoundCollection(string soundGroup)
        {
            string subFolderPath = Path.Combine(Paths.SoundsDir, soundGroup);

            var fonts = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (!Directory.Exists(subFolderPath)) return fonts;

            foreach (var soundFilePath in Directory.EnumerateFiles(subFolderPath))
            {
                if (!soundFilePath.ToLower().EndsWith("wav")) continue;

                fonts.Add(Path.GetFileNameWithoutExtension(soundFilePath), soundFilePath);
            }

            return fonts;
        }
    }
}
