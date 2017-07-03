using System.Collections.Generic;

namespace LCARS.CoreUi.Helpers
{
    public static class List
    {
        public static T Random<T>(this List<T> thisList)
        {
            return thisList[Randomizer.NextInt(thisList.Count)];
        }
    }
}
