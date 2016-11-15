using System.Collections.Generic;

namespace Algs.Tasks.DynProg
{
    public class CoinsCounter
    {
        private readonly int[] coins;
        private readonly int targetValue;
        private readonly Dictionary<string, int> memo = new Dictionary<string, int>();

        public CoinsCounter(int[] coins, int targetValue)
        {
            this.coins = coins;
            this.targetValue = targetValue;
        }

        public int GetCount()
        {
            return GetCount(coins.Length, targetValue);
        }

        public int GetCount(int maxCoins, int target)
        {
            if (maxCoins == 1)
                return target%coins[0] == 0 ? 1 : 0;
            if (target == 0)
                return 1;
            var k = maxCoins + "$$$" + target;
            int result;
            if (!memo.TryGetValue(k, out result))
            {
                var c = coins[maxCoins - 1];
                var t = target;
                result = 0;
                while (t > 0)
                {
                    result += GetCount(maxCoins - 1, t);
                    t -= c;
                }
                memo.Add(k, result);
            }
            return result;
        }
    }

    public class CoinsCounter2
    {
        private readonly int[] coins;
        private readonly int targetValue;
        private readonly Dictionary<string, int> memo = new Dictionary<string, int>();

        public CoinsCounter2(int[] coins, int targetValue)
        {
            this.coins = coins;
            this.targetValue = targetValue;
        }

        public int GetCount()
        {
            return GetCount(coins.Length, targetValue);
        }

        public int GetCount(int i, int k)
        {
            if (k == 0)
                return 1;
            if (k < 0 || i == 0)
                return 0;
            var s = i + "$$$" + k;
            int result;
            if (!memo.TryGetValue(s, out result))
                memo.Add(s, result = GetCount(i - 1, k) + GetCount(i, k - coins[i - 1]));
            return result;
        }
    }
}