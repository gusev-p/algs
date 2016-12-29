using System;
using System.Collections.Generic;
using Algs.TestUtilities;

namespace Algs.Tasks.DynProg
{
    public static class MaxWeightIndependentSet
    {
        public static void TaskMain()
        {
            var nodesCount = Input.ReadInt();
            var weights = new int[nodesCount + 1];
            for (var i = 1; i <= nodesCount; i++)
                weights[i] = Input.ReadInt();
            var subsetWeights = new int[nodesCount + 1];
            subsetWeights[0] = 0;
            subsetWeights[1] = weights[1];
            for (var i = 2; i < subsetWeights.Length; i++)
                subsetWeights[i] = Math.Max(subsetWeights[i - 1], subsetWeights[i - 2] + weights[i]);
            var maxWeightSet = new List<int>();
            for (var i = subsetWeights.Length - 1; i >= 1;)
                if (subsetWeights[i] == subsetWeights[i - 1])
                    i--;
                else
                {
                    maxWeightSet.Add(i);
                    i -= 2;
                }
            var targetVerticies = new[] {1, 2, 3, 4, 17, 117, 517, 997};
            var result = "";
            foreach (var v in targetVerticies)
            {
                var included = maxWeightSet.Contains(v);
                result += included ? '1' : '0';
            }
            Console.WriteLine(result);
        }
    }
}