using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Arrays
{
    public static class MaximumSubarraySum
    {
        public static void TaskMain()
        {
            var q = Input.ReadInt();
            for (var _ = 0; _ < q; _++)
            {
                var line = Console.ReadLine().Split(' ');
                var n = int.Parse(line[0]);
                var m = int.Parse(line[1]);
                var array = Input.ReadLongs();
            }
        }
    }
}