namespace Algs.Utilities
{
    public static class NumberHelpers
    {
        public static int Gcd(int a, int b)
        {
            while (true)
            {
                var d = a%b;
                if (d == 0)
                    return b;
                a = b;
                b = d;
            }
        }
    }
}