using System;
using System.Collections.Generic;
using Algs.TestUtilities;

namespace Algs.Tasks.Heaps
{
    public static class QHeap1
    {
        public static void TaskMain()
        {
            var q = Input.ReadInt();
            var heap = new SimpleHeap();
            for (var i = 0; i < q; i++)
            {
                var line = Console.ReadLine().Split(' ');
                var cmd = int.Parse(line[0]);
                switch (cmd)
                {
                    case 1:
                        heap.Add(int.Parse(line[1]));
                        break;
                    case 2:
                        heap.Delete(int.Parse(line[1]));
                        break;
                    case 3:
                        Console.WriteLine(heap.Min());
                        break;
                    default:
                        const string messageFormat = "unexpected cmd [{0}]";
                        throw new InvalidOperationException(string.Format(messageFormat, cmd));
                }
            }
        }

        private class SimpleHeap
        {
            private readonly int[] values = new int[100001];
            private readonly Dictionary<int, int> valueToIndexMap = new Dictionary<int, int>();
            private int count;

            public void Add(int v)
            {
                count++;
                SetValue(count, v);
                HeapifyUp(count);
            }

            public void Delete(int v)
            {
                var index = valueToIndexMap[v];
                valueToIndexMap.Remove(v);
                SetValue(index, values[count]);
                count--;
                if (values[index] < v)
                    HeapifyUp(index);
                else
                    HeapifyDown(index);
            }

            public int Min()
            {
                return values[1];
            }

            private void Swap(int i, int j)
            {
                var t = values[i];
                SetValue(i, values[j]);
                SetValue(j, t);
            }

            private void SetValue(int index, int value)
            {
                values[index] = value;
                valueToIndexMap[value] = index;
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
                    if (left <= count && values[left] < values[minIndex])
                        minIndex = left;
                    var right = left + 1;
                    if (right <= count && values[right] < values[minIndex])
                        minIndex = right;
                    if (minIndex == index)
                        break;
                    Swap(index, minIndex);
                    index = minIndex;
                }
            }
        }
    }
}