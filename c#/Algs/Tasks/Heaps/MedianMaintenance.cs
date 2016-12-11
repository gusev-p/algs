using System;

namespace Algs.Tasks.Heaps
{
    public static class MedianMaintenance
    {
        private const int itemsCount = 10000;

        public static void TaskMain()
        {
            var medianCounter = new MedianCounter(itemsCount);
            ulong mediansSum = 0;
            for (var i = 0; i < itemsCount; i++)
            {
                var number = int.Parse(Console.ReadLine());
                medianCounter.Add(number);
                mediansSum += (ulong) medianCounter.Median;
            }
            Console.WriteLine(mediansSum%10000);
        }
    }
}