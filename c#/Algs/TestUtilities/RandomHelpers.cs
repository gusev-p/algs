using System;

namespace Algs.TestUtilities
{
    public static class RandomHelpers
    {
        public static void NextInts(this Random random, int[] a, int max)
        {
            for (var i = 0; i < a.Length; i++)
                a[i] = random.Next(max) + 1;
        }
    }
}