using System;
using System.Text;

namespace Algs.Tasks.Sorting
{
    public static class QuickSorter
    {
        public static void quickSort(int[] ar)
        {
            quickSort(ar, 0, ar.Length - 1);
        }

        private static void quickSort(int[] ar, int lo, int hi)
        {
            if (lo >= hi)
                return;
            var mid = partition(ar, lo, hi);
            quickSort(ar, lo, mid - 1);
            quickSort(ar, mid + 1, hi);
            printArraySegment(ar, lo, hi);
        }

        private static void printArraySegment(int[] ar, int lo, int hi)
        {
            var b = new StringBuilder();
            var isFirst = true;
            for (var i = lo; i <= hi; i++)
            {
                if (isFirst)
                    isFirst = false;
                else
                    b.Append(' ');
                b.Append(ar[i]);
            }
            Console.WriteLine(b);
        }

        private static int partition(int[] ar, int lo, int hi)
        {
            var less = new int[hi - lo + 1];
            var greater = new int[hi - lo + 1];
            int l = -1, g = -1;
            var pivot = ar[lo];
            for (var i = lo + 1; i <= hi; i++)
            {
                if (ar[i] < pivot)
                    less[++l] = ar[i];
                else if (ar[i] > pivot)
                    greater[++g] = ar[i];
            }
            var index = lo;
            for (var i = 0; i <= l; i++)
                ar[index++] = less[i];
            var result = index;
            ar[index++] = pivot;
            for (var i = 0; i <= g; i++)
                ar[index++] = greater[i];
            return result;
        }
    }
}