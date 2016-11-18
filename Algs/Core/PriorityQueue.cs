using System;

namespace Algs.Core
{
    public class PriorityQueue<T>
    {
        private readonly Func<T, T, MostPriority> prioritizer;
        public event Action<T, int> OnHandleChanged;
        private readonly T[] values;

        public PriorityQueue(int maxItemsCount, Func<T, T, MostPriority> prioritizer)
        {
            this.prioritizer = prioritizer;
            values = new T[maxItemsCount + 1];
        }

        public int Count { get; private set; }

        public void Insert(T value)
        {
            if (Count == values.Length - 1)
                throw new InvalidOperationException("overflow");
            Count++;
            SetValue(Count, value);
            Promote(Count);
        }

        public int Promote(int handle)
        {
            var index = handle;
            while (true)
            {
                var parentIndex = index >> 1;
                if (parentIndex == 0 || Prioritize(index, parentIndex) != MostPriority.First)
                    return index;
                Exchange(parentIndex, index);
                index = parentIndex;
            }
        }

        public T ExtractTop()
        {
            var result = values[1];
            NotifyHandleChanged(result, -1);
            SetValue(1, values[Count]);
            Count--;
            var index = 1;
            while (true)
            {
                var largestIndex = index;
                var leftIndex = index << 1;
                if (leftIndex <= Count && Prioritize(leftIndex, index) == MostPriority.First)
                    largestIndex = leftIndex;
                var rightIndex = leftIndex | 1;
                if (rightIndex <= Count && Prioritize(rightIndex, largestIndex) == MostPriority.First)
                    largestIndex = rightIndex;
                if (largestIndex == index)
                    break;
                Exchange(index, largestIndex);
                index = largestIndex;
            }
            return result;
        }

        private void Exchange(int i, int j)
        {
            var t = values[i];
            SetValue(i, values[j]);
            SetValue(j, t);
        }

        private void SetValue(int index, T value)
        {
            values[index] = value;
            NotifyHandleChanged(value, index);
        }

        private MostPriority Prioritize(int targetIndex, int comparandIndex)
        {
            return prioritizer(values[targetIndex], values[comparandIndex]);
        }

        private void NotifyHandleChanged(T value, int newHandle)
        {
            if (OnHandleChanged != null)
                OnHandleChanged(value, newHandle);
        }
    }
}