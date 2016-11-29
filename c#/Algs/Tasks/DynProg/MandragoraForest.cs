using System;
using System.Linq;
using Algs.TestUtilities;

namespace Algs.Tasks.DynProg
{
    public static class MandragoraForest
    {
        public static void TaskMain()
        {
            var t = Input.ReadInt();
            for (var _ = 0; _ < t; _++)
            {
                Input.ReadInt();
                var h = Input.ReadLongs();
                var maxExperience = CalculateMaxExperience(h);
                Console.WriteLine(maxExperience);
            }
        }

        private static long CalculateMaxExperience(long[] health)
        {
            Array.Sort(health);
            var s = 1;
            var totalHealth = health.Sum();
            var battleProfit = totalHealth;
            foreach (var h in health)
            {
                var eatProfit = (s + 1)*(totalHealth - h);
                if (eatProfit <= battleProfit)
                    break;
                s++;
                totalHealth -= h;
                battleProfit = s*totalHealth;
            }
            return battleProfit;
        }
    }
}