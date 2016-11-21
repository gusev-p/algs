using System;

namespace Algs.Tasks.Sorting
{
    public class MergeSorter
    {
        private readonly int[] data;
        private readonly int[] temp;

        public MergeSorter(int[] data)
        {
            this.data = data;
            temp = new int[data.Length];
        }

        public void Sort()
        {
            Sort(0, data.Length - 1);
        }

        public long InversionsCount { get; private set; }

        private void Sort(int left, int right)
        {
            if (left >= right)
                return;
            var middle = left + (right - left)/2;
            Sort(left, middle);
            Sort(middle + 1, right);
            Merge(left, middle, right);
        }

        private void Merge(int leftStart, int middle, int rightEnd)
        {
            var left = leftStart;
            var right = middle + 1;
            var index = leftStart;
            while (left <= middle || right <= rightEnd)
            {
                if (left > middle)
                    temp[index] = data[right++];
                else if (right > rightEnd)
                    temp[index] = data[left++];
                else if (data[left] <= data[right])
                    temp[index] = data[left++];
                else
                {
                    temp[index] = data[right++];
                    InversionsCount += middle - left + 1;
                }
                index++;
            }
            Array.Copy(temp, leftStart, data, leftStart, rightEnd - leftStart + 1);
        }
    }
}