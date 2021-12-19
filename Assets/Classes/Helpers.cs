using System;
using System.Collections.Generic;

namespace Assets.Classes
{
    public static class Helpers
    {
        public static void Shuffle<T>(this List<T> list)
        {
            var randomNumberGenerator = new Random();
            var n = list.Count;

            while (n > 1)
            {
                var k = randomNumberGenerator.Next(--n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
