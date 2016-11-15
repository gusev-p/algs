namespace Algs.Tasks.Sorting
{
    public static class QuickSorter2
    {
        public static void QuickSort(int[] array)
        {
            QuickSort(array, 0, array.Length - 1);
        }

        public static long fuckingComparisonsCount;

        private static void QuickSort(int[] array, int left, int right)
        {
            if (left >= right)
                return;
            //Swap(array, left, right);
            Swap(array, left, GetMedian(array, left, right));
            fuckingComparisonsCount += (right - left + 1) - 1;
            var mid = Partition(array, left, right);
            QuickSort(array, left, mid - 1);
            QuickSort(array, mid + 1, right);
        }

        private static int Partition(int[] array, int left, int right)
        {
            var pivot = array[left];
            var i = left + 1;
            for (var j = i; j <= right; j++)
                if (array[j] < pivot)
                {
                    Swap(array, i, j);
                    i++;
                }
            Swap(array, i - 1, left);
            return i - 1;
        }

        private static int GetMedian(int[] array, int left, int right)
        {
            var l = array[left];
            var r = array[right];
            var mid = left + (right - left)/2;
            var m = array[mid];
            if (l < r)
            {
                if (m < l)
                    return left;
                if (m < r)
                    return mid;
                return right;
            }
            if (m < r)
                return right;
            if (m < l)
                return mid;
            return left;
        }

        private static void Swap(int[] array, int i, int j)
        {
            var t = array[i];
            array[i] = array[j];
            array[j] = t;
        }
    }
}