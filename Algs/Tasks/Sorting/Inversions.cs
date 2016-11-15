namespace Algs.Tasks.Sorting
{
    public static class Inversions
    {
        public static long Count(int[] a)
        {
            var b = new int[a.Length];
            var aux = new int[a.Length];
            for (var i = 0; i < a.Length; i++)
                b[i] = a[i];
            return Count(a, b, aux, 0, a.Length - 1);
        }

        private static long Merge(int[] a, int[] aux, int lo, int mid, int hi)
        {
            long inversions = 0;
            for (var k = lo; k <= hi; k++)
                aux[k] = a[k];
            int i = lo, j = mid + 1;
            for (var k = lo; k <= hi; k++)
            {
                if (i > mid) a[k] = aux[j++];
                else if (j > hi) a[k] = aux[i++];
                else if (aux[j] < aux[i])
                {
                    a[k] = aux[j++];
                    inversions += (mid - i + 1);
                }
                else a[k] = aux[i++];
            }
            return inversions;
        }

        private static long Count(int[] a, int[] b, int[] aux, int lo, int hi)
        {
            long inversions = 0;
            if (hi <= lo) return 0;
            var mid = lo + (hi - lo)/2;
            inversions += Count(a, b, aux, lo, mid);
            inversions += Count(a, b, aux, mid + 1, hi);
            inversions += Merge(b, aux, lo, mid, hi);
            //if(inversions != brute(a, lo, hi))
            //    throw new in
            return inversions;
        }
    }
}