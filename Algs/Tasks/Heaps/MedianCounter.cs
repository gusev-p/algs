namespace Algs.Tasks.Heaps
{
    public class MedianCounter
    {
        private readonly Heap maxHeap;
        private readonly Heap minHeap;

        public MedianCounter(int maxCount)
        {
            maxHeap = new Heap(maxCount, true);
            minHeap = new Heap(maxCount, false);
        }

        public void Add(int value)
        {
            if (value <= Median)
                maxHeap.Add(value);
            else
                minHeap.Add(value);
            if (maxHeap.Count > minHeap.Count + 1)
                minHeap.Add(maxHeap.ExtractTop());
            else if (minHeap.Count > maxHeap.Count + 1)
                maxHeap.Add(minHeap.ExtractTop());
            if (maxHeap.Count == minHeap.Count)
                Median = ((double) minHeap.Top + maxHeap.Top)/2;
            else if (maxHeap.Count > minHeap.Count)
                Median = maxHeap.Top;
            else
                Median = minHeap.Top;
        }

        public double Median { get; private set; }
    }
}