using System.Collections.Generic;

namespace Algs.Core
{
    public static class ArrayHelpers
    {
        public static int FindFirstGreaterOrEqual(int[] array, int target)
        {
            var left = 0;
            var right = array.Length - 1;
            var result = -1;
            while (left <= right)
            {
                var mid = left + (right - left)/2;
                var midValue = array[mid];
                if (target <= midValue)
                {
                    right = mid - 1;
                    result = mid;
                }
                else
                    left = mid + 1;
            }
            return result;
        }

        public static int FindLastSmallerOrEqual(int[] array, int target)
        {
            var left = 0;
            var right = array.Length - 1;
            var result = -1;
            while (left <= right)
            {
                var mid = left + (right - left)/2;
                var midValue = array[mid];
                if (target < midValue)
                    right = mid - 1;
                else
                {
                    result = mid;
                    left = mid + 1;
                }
            }
            return result;
        }

        public static int BinarySearch<T>(this T[] array, T item,
            Occurence occurence = Occurence.First,
            IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
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