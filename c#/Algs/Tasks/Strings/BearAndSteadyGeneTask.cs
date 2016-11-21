using System;

namespace Algs.Tasks.Strings
{
    public class BearAndSteadyGeneTask
    {
        private readonly string source;

        public BearAndSteadyGeneTask(string source)
        {
            this.source = source;
        }

        public int Execute()
        {
            var totalCounts = new int[4];
            var targetCounts = new int[4];
            foreach (var t in source)
                totalCounts[GetIndex(t)]++;
            for (var i = 0; i < totalCounts.Length; i++)
                targetCounts[i] = totalCounts[i] > source.Length/4
                    ? totalCounts[i] - source.Length/4
                    : 0;
            var totalDiff = 0;
            foreach (var t in targetCounts)
                totalDiff += t;
            if (totalDiff == 0)
                return 0;
            var counts = new int[4, source.Length];
            counts[GetIndex(source[0]), 0] = 1;
            for (var i = 1; i < source.Length; i++)
            {
                for (var j = 0; j < 4; j++)
                    counts[j, i] = counts[j, i - 1];
                counts[GetIndex(source[i]), i]++;
            }
            var currentCounts = new int[4];
            for (var currentLen = totalDiff; currentLen < source.Length; currentLen++)
            {
                var left = 0;
                var right = currentLen - 1;
                while (right < source.Length)
                {
                    for (var j = 0; j < 4; j++)
                        currentCounts[j] = counts[j, right];
                    if (left > 0)
                        for (var j = 0; j < 4; j++)
                            currentCounts[j] -= counts[j, left - 1];
                    var currentDiff = 0;
                    for (var i = 0; i < currentCounts.Length; i++)
                    {
                        var currentCount = currentCounts[i];
                        var targetCount = targetCounts[i];
                        if (targetCount > currentCount)
                            currentDiff += targetCount - currentCount;
                    }
                    if (currentDiff == 0)
                        return currentLen;
                    left += currentDiff;
                    right += currentDiff;
                }
            }
            return source.Length;
        }

        private static int GetIndex(char c)
        {
            switch (c)
            {
                case 'A':
                    return 0;
                case 'C':
                    return 1;
                case 'T':
                    return 2;
                case 'G':
                    return 3;
            }
            throw new InvalidOperationException("assertion failure");
        }
    }
}