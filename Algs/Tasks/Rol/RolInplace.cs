using Algs.Utilities;

namespace Algs.Tasks.Rol
{
    public class RolInplace
    {
        public static void Execute(int[] a, int n, int d)
        {
            if (d == n)
                return;
            var g = NumberHelpers.Gcd(n, d);
            for (var i = 0; i < g; i++)
            {
                var j = i;
                var prev = a[i];
                do
                {
                    j -= d;
                    if (j < 0)
                        j += n;
                    var t = a[j];
                    a[j] = prev;
                    prev = t;
                } while (j != i);
            }
        }
    }
}