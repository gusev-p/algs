using System.Collections.Generic;

namespace Algs.Tasks.DynProg
{
    public class CoinsCounter
    {
        private readonly int[] coins;
        private readonly int n;
        private readonly Dictionary<string, long> memo = new Dictionary<string, long>();

        public CoinsCounter(int[] coins, int n)
        {
            this.coins = coins;
            this.n = n;
        }

        public long GetChangeCount()
        {
            return GetChangeCount(coins.Length, n);
        }

        private long GetChangeCount(int i, int k)
        {
            if (i == 1)
                return k%coins[0] == 0 ? 1 : 0;
            if (k == 0)
                return 1;
            if (k < 0)
                return 0;
            var key = i + "$$$" + k;
            long result;
            if (!memo.TryGetValue(key, out result))
            {
                result = GetChangeCount(i - 1, k) + GetChangeCount(i, k - coins[i - 1]);
                memo.Add(key, result);
            }
            return result;
        }
    }
}