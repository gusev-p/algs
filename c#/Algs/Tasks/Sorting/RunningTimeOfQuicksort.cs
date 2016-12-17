using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Sorting
{
    public static class RunningTimeOfQuicksort
    {
        public static void TaskMain()
        {
            Input.ReadInt();
            var numbers = Input.ReadInts();
            var numbersCopy = numbers.Copy();
            var quickSort = new QuickSort(numbers);
            var insertionSort = new InsertionSort(numbersCopy);
            quickSort.Apply();
            insertionSort.Apply();
            Console.WriteLine(insertionSort.SwapsCount - quickSort.SwapsCount);
        }

        private abstract class AbstractSort
        {
            protected readonly int[] numbers;

            protected AbstractSort(int[] numbers)
            {
                this.numbers = numbers;
            }

            public int SwapsCount { get; protected set; }

            public abstract void Apply();

            protected void Swap(int i, int j)
            {
                SwapInt(i, j);
                SwapsCount++;
            }

            protected void SwapInt(int i, int j)
            {
                var t = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = t;
            }
        }

        private class InsertionSort : AbstractSort
        {
            public InsertionSort(int[] numbers)
                : base(numbers)
            {
            }

            public override void Apply()
            {
                for (var i = 1; i < numbers.Length; i++)
                {
                    var value = numbers[i];
                    var index = i - 1;
                    while (index >= 0 && numbers[index] > value)
                    {
                        numbers[index + 1] = numbers[index];
                        SwapsCount++;
                        index--;
                    }
                    numbers[index + 1] = value;
                }
            }
        }

        private class QuickSort : AbstractSort
        {
            public QuickSort(int[] numbers)
                : base(numbers)
            {
            }

            public override void Apply()
            {
                Apply(0, numbers.Length - 1);
            }

            private void Apply(int left, int right)
            {
                if (left >= right)
                    return;
                var mid = Partition(left, right);
                Apply(left, mid - 1);
                Apply(mid + 1, right);
            }

            private int Partition(int left, int right)
            {
                var pivot = numbers[right];
                var i = left;
                for (var j = left; j < right; j++)
                    if (numbers[j] < pivot)
                    {
                        Swap(i, j);
                        i++;
                    }
                Swap(right, i);
                return i;
            }
        }
    }
}