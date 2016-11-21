using System;

namespace Algs.Tasks.Numbers
{
    public static class PrimalityChecker
    {
        public static bool IsPrime(long number)
        {
            if (number <= 1)
                return false;
            if ((number & 1) == 0)
                return false;
            var limit = (long) Math.Sqrt(number) + 1;
            for (long i = 3; i <= limit; i += 2)
                if (number%i == 0)
                    return false;
            return true;
        }
    }
}