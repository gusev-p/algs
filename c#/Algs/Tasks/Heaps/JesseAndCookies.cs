using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Heaps
{
    public static class JesseAndCookies
    {
        public static void TaskMain()
        {
            var line0 = Input.ReadInts();
            var k = line0[1];
            var line1 = Input.ReadInts();
            var heap = SimpleMinHeap.CreateFrom(line1);
            var operationsCount = 0;
            while (heap.Count >= 2 && heap.Top() < k)
            {
                var v1 = heap.RemoveTop();
                var v2 = heap.RemoveTop();
                var newValue = v1 + 2*v2;
                heap.Add(newValue);
                operationsCount++;
            }
            if (heap.Top() < k)
                operationsCount = -1;
            Console.WriteLine(operationsCount);
        }

        private class SimpleMinHeap
        {
            private readonly int[] values;
            public int Count { get; private set; }

            private SimpleMinHeap(int[] values)
            {
                this.values = values;
            }

            public static SimpleMinHeap CreateFrom(int[] array)
            {
                var copy = new int[array.Length + 1];
                Array.Copy(array, 0, copy, 1, array.Length);
                var result = new SimpleMinHeap(copy) {Count = array.Length};
                var start = array.Length >> 1;
                for (var i = start; i > 0; i--)
                    result.HeapifyDown(i);
                return result;
            }

            public void Add(int v)
            {
                Count++;
                values[Count] = v;
                HeapifyUp(Count);
            }

            public int Top()
            {
                return values[1];
            }

            public int RemoveTop()
            {
                var result = values[1];
                values[1] = values[Count];
                Count--;
                HeapifyDown(1);
                return result;
            }

            private void HeapifyUp(int index)
            {
                while (true)
                {
                    var parentIndex = index >> 1;
                    if (parentIndex == 0 || values[parentIndex] <= values[index])
                        break;
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
            }

            private void HeapifyDown(int index)
            {
                while (true)
                {
                    var minIndex = index;
                    var left = index << 1;
                    if (left <= Count && values[left] < values[minIndex])
                        minIndex = left;
                    var right = left + 1;
                    if (right <= Count && values[right] < values[minIndex])
                        minIndex = right;
                    if (minIndex == index)
                        break;
                    Swap(index, minIndex);
                    index = minIndex;
                }
            }

            private void Swap(int i, int j)
            {
                var t = values[i];
                values[i] = values[j];
                values[j] = t;
            }
        }
    }
}