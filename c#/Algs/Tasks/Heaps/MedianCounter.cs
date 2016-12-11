using Algs.Core;

namespace Algs.Tasks.Heaps
{
    public class MedianCounter
    {
        private readonly PriorityQueue<int> maxHeap;
        private readonly PriorityQueue<int> minHeap;

        public MedianCounter(int maxCount)
        {
            maxHeap = PriorityQueue<int>.Max(maxCount);
            minHeap = PriorityQueue<int>.Min(maxCount);
        }

        public void Add(int value)
        {
            if (value <= Median)
                maxHeap.Insert(value);
            else
                minHeap.Insert(value);
            if (maxHeap.Count > minHeap.Count + 1)
                minHeap.Insert(maxHeap.ExtractTop());
            else if (minHeap.Count > maxHeap.Count + 1)
                maxHeap.Insert(minHeap.ExtractTop());
            if (maxHeap.Count == minHeap.Count)
                Median = maxHeap.Top;
            else if (maxHeap.Count > minHeap.Count)
                Median = maxHeap.Top;
            else
                Median = minHeap.Top;
        }

        public int Median { get; private set; }
    }
}