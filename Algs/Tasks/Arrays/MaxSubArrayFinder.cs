using System;

namespace Algs.Tasks.Arrays
{
    public static class MaxSubArrayFinder
    {
        public static int FindMaxItemIndex(int[] array)
        {
            var maxIndex = 0;
            long max = array[0];
            for (var i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    maxIndex = i;
                    max = array[i];
                }
            }
            return maxIndex;
        }

        public static Tuple<int, int> FindMaxSubArray(int[] array)
        {
            int maxStart = -1, maxFinish = -1;
            long maxSum = 0;
            int start = -1, finish = -1;
            long sum = 0;
            for (var i = 0; i < array.Length; i++)
            {
                if (start >= 0)
                {
                    if (array[i] > 0)
                        finish = i;
                    else if (sum > maxSum)
                    {
                        maxStart = start;
                        maxFinish = finish;
                        maxSum = sum;
                    }
                    sum += array[i];
                    if (sum <= 0)
                        start = -1;
                }
                else if (array[i] > 0)
                {
                    start = i;
                    finish = i;
                    sum = array[i];
                }
            }
            if (sum > maxSum)
            {
                maxStart = start;
                maxFinish = finish;
            }
            return Tuple.Create(maxStart, maxFinish);
        }
    }
}