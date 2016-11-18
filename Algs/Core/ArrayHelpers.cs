using System.Collections.Generic;

namespace Algs.Core
{
    public static class ArrayHelpers
    {
        public static int BinarySearch<T>(this T[] array, T item, IComparer<T> comparer, Occurence occurence)
        {
            var left = 0;
            var right = array.Length - 1;
            var result = -1;
            while (left <= right)
            {
                var m = left + (right - left)/2;
                var cmp = comparer.Compare(item, array[m]);
                if (cmp < 0)
                    right = m - 1;
                else if (cmp > 0)
                    left = m + 1;
                else
                {
                    result = m;
                    if (occurence == Occurence.First)
                        right = m - 1;
                    else
                        left = m + 1;
                }
            }
            return result;
        }
    }
}