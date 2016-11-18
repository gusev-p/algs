using System;
using Algs.Core;
using Algs.TestUtilities;

namespace Algs.Tasks.Arrays
{
    public static class SparseArrays
    {
        public static void TaskMain()
        {
            var n = Input.ReadInt();
            var strings = new string[n];
            for (var i = 0; i < n; i++)
                strings[i] = Console.ReadLine();
            Array.Sort(strings, StringComparer.Ordinal);
            var q = Input.ReadInt();
            for (var i = 0; i < q; i++)
            {
                var s = Console.ReadLine();
                int count;
                var first = strings.BinarySearch(s, StringComparer.Ordinal, Occurence.First);
                if (first == -1)
                    count = 0;
                else
                {
                    var last = strings.BinarySearch(s, StringComparer.Ordinal, Occurence.Last);
                    count = last - first + 1;
                }
                Console.WriteLine(count);
            }
        }
    }
}