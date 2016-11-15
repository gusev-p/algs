namespace Algs.Tasks.Sorting
{
    public static class InsertionSorter
    {
        public static void InsertionSort(int[] array)
        {
            for (var i = 1; i < array.Length; i++)
            {
                var value = array[i];
                var index = i - 1;
                while (index >= 0 && array[index] > value)
                {
                    array[index + 1] = array[index];
                    index--;
                }
                array[index + 1] = value;
            }
        }
    }
}