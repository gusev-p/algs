using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Numbers
{
    public static class AbsolutePermutation
    {
        public static void TaskMain()
        {
            var t = Input.ReadInt();
            for (var _ = 0; _ < t; _++)
            {
                var line = Console.ReadLine().Split(' ');
                var n = int.Parse(line[0]);
                var k = int.Parse(line[1]);
                var permutation = GeneratePermulation(n, k);
                if (permutation == null)
                    Console.WriteLine(-1);
                else
                    Console.WriteLine(string.Join(" ", permutation));
            }
        }

        private static int[] GeneratePermulation(int n, int k)
        {
            if (k != 0 && n%(2*k) != 0)
                return null;
            var result = new int[n];
            var remaining = k;
            var forward = true;
            for (var i = 0; i < n; i++)
            {
                result[forward ? i + k : i - k] = i + 1;
                if (--remaining <= 0)
                {
                    forward = !forward;
                    remaining = k;
                }
            }
            return result;
        }
    }
}