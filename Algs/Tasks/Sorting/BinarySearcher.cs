namespace Algs.Tasks.Sorting
{
    public static class BinarySearcher
    {
        public static int FindFirst(int[] array, int value)
        {
            var left = 0;
            var right = array.Length - 1;
            while (left < right)
            {
                var mid = left + (right - left)/2;
                var v = array[mid];
                if (v >= value)
                    right = mid;
                else
                    left = mid + 1;
            }
            return array[left] == value ? left : -1;
        }
    }
}