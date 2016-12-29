using System;
using System.Collections.Generic;

namespace Algs.Core
{
    public class PriorityQueue<T>
    {
        private readonly Func<T, T, MostPriority> prioritizer;
        private readonly Action<T, int> onHandleChanged;
        private readonly T[] values;

        public PriorityQueue(int maxItemsCount, Func<T, T, MostPriority> prioritizer,
            Action<T, int> onHandleChanged = null)
            : this(new T[maxItemsCount + 1], prioritizer, onHandleChanged)
        {
        }

        private PriorityQueue(T[] values, Func<T, T, MostPriority> prioritizer, Action<T, int> onHandleChanged)
        {
            this.values = values;
            this.prioritizer = prioritizer;
            this.onHandleChanged = onHandleChanged;
        }

        public static PriorityQueue<T> Min(int maxItemsCount, IComparer<T> comparer = null,
            Action<T, int> onHandleChanged = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            return new PriorityQueue<T>(maxItemsCount, delegate(T v1, T v2)
            {
                var cmp = comparer.Compare(v1, v2);
                if (cmp < 0)
                    return MostPriority.First;
                if (cmp > 0)
                    return MostPriority.Second;
                return MostPriority.Both;
            }, onHandleChanged);
        }

        public static PriorityQueue<T> Max(int maxItemsCount, IComparer<T> comparer = null,
            Action<T, int> onHandleChanged = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            return new PriorityQueue<T>(maxItemsCount, delegate(T v1, T v2)
            {
                var cmp = comparer.Compare(v1, v2);
                if (cmp > 0)
                    return MostPriority.First;
                if (cmp < 0)
                    return MostPriority.Second;
                return MostPriority.Both;
            }, onHandleChanged);
        }

        public static PriorityQueue<T> Create(T[] values, Func<T, T, MostPriority> prioritizer,
            Action<T, int> onHandleChanged = null)
        {
            var heapValues = new T[values.Length + 1];
            Array.Copy(values, 0, heapValues, 1, values.Length);
            var result = new PriorityQueue<T>(heapValues, prioritizer, onHandleChanged)
            {
                Count = values.Length
            };
            if (onHandleChanged != null)
                for (var i = 1; i < heapValues.Length; i++)
                    onHandleChanged(heapValues[i], i);
            for (var i = values.Length/2; i >= 1; i--)
                result.HeapifyDown(i);
            return result;
        }

        public int Count { get; private set; }

        public void Insert(T value)
        {
            if (Count == values.Length - 1)
                throw new InvalidOperationException("overflow");
            Count++;
            SetValue(Count, value);
            HeapifyUp(Count);
        }

        public T Top => values[1];

        public T ExtractTop()
        {
            var result = values[1];
            NotifyHandleChanged(result, -1);
            SetValue(1, values[Count]);
            Count--;
            HeapifyDown(1);
            return result;
        }

        public void Delete(int handle)
        {
            var oldValue = values[handle];
            NotifyHandleChanged(oldValue, -1);
            SetValue(handle, values[Count]);
            Count--;
            var prioritization = prioritizer(values[handle], oldValue);
            if (prioritization == MostPriority.First)
                HeapifyUp(handle);
            else if(prioritization == MostPriority.Second)
                HeapifyDown(handle); 
        }

        public void HeapifyDown(int index)
        {
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
        }

        public void HeapifyUp(int index)
        {
            while (true)
            {
                var parentIndex = index >> 1;
                if (parentIndex == 0 || Prioritize(index, parentIndex) != MostPriority.First)
                    return;
                Exchange(parentIndex, index);
                index = parentIndex;
            }
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
            onHandleChanged?.Invoke(value, newHandle);
        }
    }
}