using System;

namespace LCARS.CoreUi.Helpers
{
    public static class Randomizer
    {
        // provide a global randomizer so there are no seed issues with successive new Random objects
        private static Random random = new Random();
        public static int NextInt() { return random.Next(); }
        public static int NextInt(int max) { return random.Next(max); }
        public static int NextInt(int min, int max) { return random.Next(min, max); }
        public static double NextDouble() { return random.NextDouble(); }
        public static bool NextBool() { return random.NextDouble() > .5; }
    }
}
