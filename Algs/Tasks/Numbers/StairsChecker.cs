namespace Algs.Tasks.Numbers
{
    public static class StairsChecker
    {
        public static long CountNumberOfClimbings(long n)
        {
            if (n == 1)
                return 1;
            if (n == 2)
                return 2;
            if (n == 3)
                return 4;
            long prev1 = 1;
            long prev2 = 2;
            long prev3 = 4;
            for (long i = 4; i <= n; i++)
            {
                var newValue = prev1 + prev2 + prev3;
                prev1 = prev2;
                prev2 = prev3;
                prev3 = newValue;
            }
            return prev3;
        }
    }
}