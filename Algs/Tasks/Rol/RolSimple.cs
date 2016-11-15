namespace Algs.Tasks.Rol
{
    public class RolSimple
    {
        public static int[] Execute(int[] a, int n, int d)
        {
            var b = new int[n];
            for (var i = 0; i < n; i++)
            {
                var j = i - d;
                if (j < 0)
                    j += n;
                b[j] = a[i];
            }
            return b;
        }
    }
}