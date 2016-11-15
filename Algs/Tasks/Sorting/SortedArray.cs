using System;

namespace Algs.Tasks.Sorting
{
    public static class SortedArray
    {
        public static Tuple<int, int> FindItemsWithSum(int[] array, int sum)
        {
            var i = 0;
            var j = array.Length - 1;
            while (i < j)
            {
                var s = array[i] + array[j];
                if (s == sum)
                    return Tuple.Create(i, j);
                if (s > sum)
                    j--;
                else
                    i++;
            }
            throw new InvalidOperationException("assertion failure");
        }
    }
}