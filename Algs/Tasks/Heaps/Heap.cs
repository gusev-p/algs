namespace Algs.Tasks.Heaps
{
    public class Heap
    {
        private readonly bool isMax;
        private readonly int[] values;

        public Heap(int maxCount, bool isMax)
        {
            this.isMax = isMax;
            values = new int[maxCount + 1];
        }

        public void Add(int value)
        {
            Count++;
            values[Count] = value;
            var index = Count;
            while (true)
            {
                var parentIndex = index >> 1;
                if (parentIndex == 0 || LargerThan(values[parentIndex], values[index]))
                    break;
                var t = values[parentIndex];
                values[parentIndex] = values[index];
                values[index] = t;
                index = parentIndex;
            }
        }

        public int Top
        {
            get { return values[1]; }
        }

        public int ExtractTop()
        {
            var result = values[1];
            values[1] = values[Count];
            Count--;
            Heapify();
            return result;
        }

        public int Count { get; private set; }

        private void Heapify()
        {
            var index = 1;
            while (true)
            {
                var largestIndex = index;
                var largestValue = values[largestIndex];
                var leftIndex = index << 1;
                if (leftIndex <= Count && LargerThan(values[leftIndex], largestValue))
                {
                    largestIndex = leftIndex;
                    largestValue = values[leftIndex];
                }
                var rightIndex = leftIndex | 1;
                if (rightIndex <= Count && LargerThan(values[rightIndex], largestValue))
                {
                    largestIndex = rightIndex;
                    largestValue = values[rightIndex];
                }
                if (largestIndex == index)
                    break;
                values[largestIndex] = values[index];
                values[index] = largestValue;
                index = largestIndex;
            }
        }

        private bool LargerThan(int v1, int v2)
        {
            return isMax ? v1 >= v2 : v2 >= v1;
        }
    }
}