using System;
using System.Collections.Generic;

namespace Algs.Tasks.Arrays
{
    public static class MissingNumbers
    {
        public static void TaskMain()
        {
            Console.ReadLine();
            var aStrings = Console.ReadLine().Split(' ');
            var a = Array.ConvertAll(aStrings, int.Parse);
            Console.ReadLine();
            var bStrings = Console.ReadLine().Split(' ');
            var b = Array.ConvertAll(bStrings, int.Parse);
            var diff = Diff(a, b);
            Console.WriteLine(string.Join(" ", diff));
        }

        private static List<int> Diff(int[] a, int[] b)
        {
            var bCounts = new int[100];
            var bMin = int.MaxValue;
            foreach (var t in b)
                if (t < bMin)
                    bMin = t;
            foreach (var t in b)
                bCounts[t - bMin]++;
            foreach (var t in a)
                bCounts[t - bMin]--;
            var result = new List<int>();
            for (var i = 0; i < bCounts.Length; i++)
                if (bCounts[i] > 0)
                    result.Add(i + bMin);
            return result;
        }
    }
}