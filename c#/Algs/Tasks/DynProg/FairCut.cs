using System;
using Algs.TestUtilities;

namespace Algs.Tasks.DynProg
{
    public static class FairCut
    {
        public static void TaskMain()
        {
            var ints = Input.ReadInts();
            var k = ints[1];
            var a = Input.ReadLongs();
            var minUnfairness = MinUnfairness(a, k);
            Console.WriteLine(minUnfairness);
        }

        private static long MinUnfairness(long[] a, int k)
        {
            Array.Sort(a);
            var d = new long[a.Length];
            long s = 0;
            for (var i = 1; i < d.Length; i++)
                d[i] = s += a[i] - a[i - 1];
            long sRight = 0;
            for (var i = 1; i < d.Length; i++)
                sRight += d[i];
            long sLeft = 0;
            for (var i = 0; i < d.Length; i++)
            {
                sRight -= d[i];
                var newValue = d[i]*(2*i - a.Length + 1) + sRight - sLeft;
                sLeft += d[i];
                d[i] = newValue;
            }
            var b = new long[d.Length];
            long f = 0;
            for (var l = 0; l < k; l++)
            {
                var diff = long.MaxValue;
                var nextIndex = -1;
                for (var j = 0; j < d.Length; j++)
                {
                    if (d[j] == -1)
                        continue;
                    var v = d[j] - b[j];
                    if (v < diff)
                    {
                        diff = v;
                        nextIndex = j;
                    }
                }
                f += diff;
                for (var i = 0; i < d.Length; i++)
                {
                    if (d[i] == -1)
                        continue;
                    var v = Math.Abs(a[i] - a[nextIndex]);
                    if (i == nextIndex)
                        d[i] = -1;
                    else
                        d[i] -= v;
                    b[i] += v;
                }
            }
            return f;
        }
    }
}