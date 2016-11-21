using System;
using System.Text;

namespace Algs.Tasks.Numbers
{
    //todo remove stupid array allocation on each iteration
    public static class KaratsubaMultiplier
    {
        private static readonly byte[,] mulTable0 =
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
            {0, 2, 4, 6, 8, 0, 2, 4, 6, 8},
            {0, 3, 6, 9, 2, 5, 8, 1, 4, 7},
            {0, 4, 8, 2, 6, 0, 4, 8, 2, 6},
            {0, 5, 0, 5, 0, 5, 0, 5, 0, 5},
            {0, 6, 2, 8, 4, 0, 6, 2, 8, 4},
            {0, 7, 4, 1, 8, 5, 2, 9, 6, 3},
            {0, 8, 6, 4, 2, 0, 8, 6, 4, 2},
            {0, 9, 8, 7, 6, 5, 4, 3, 2, 1}
        };

        private static readonly byte[,] mulTable1 =
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            {0, 0, 0, 0, 1, 1, 1, 2, 2, 2},
            {0, 0, 0, 1, 1, 2, 2, 2, 3, 3},
            {0, 0, 1, 1, 2, 2, 3, 3, 4, 4},
            {0, 0, 1, 1, 2, 3, 3, 4, 4, 5},
            {0, 0, 1, 2, 2, 3, 4, 4, 5, 6},
            {0, 0, 1, 2, 3, 4, 4, 5, 6, 7},
            {0, 0, 1, 2, 3, 4, 5, 6, 7, 8}
        };

        public static string Multiply(string x, string y)
        {
            return AsString(Multiply(AsBytes(x), AsBytes(y)));
        }

        public static byte[] AsBytes(string digits)
        {
            var result = new byte[digits.Length];
            for (int i = 0, j = digits.Length - 1; i < result.Length; i++, j--)
                result[i] = (byte) (digits[j] - '0');
            return result;
        }

        public static string AsString(byte[] digits)
        {
            var result = new StringBuilder {Length = digits.Length};
            for (int i = 0, j = digits.Length - 1; i < result.Length; i++, j--)
                result[i] = (char) ('0' + digits[j]);
            return result.ToString();
        }

        public static byte[] Multiply(byte[] x, byte[] y)
        {
            var result = new byte[x.Length + y.Length];
            if (x.Length > y.Length)
            {
                var t = x;
                x = y;
                y = t;
            }
            if (x.Length < y.Length)
            {
                var newArray = new byte[y.Length];
                Array.Copy(x, newArray, x.Length);
                x = newArray;
            }
            Multiply(Number.From(x), Number.From(y), Number.From(result));
            return result;
        }

        private static void Multiply(Number x, Number y, Number result)
        {
            if (x.Length <= 3)
            {
                Number.MulGradeSchool(x, y, result);
                return;
            }
            var kl = x.Length/2;
            var kr = x.Length - kl;
            var a = x.Slice(x.left, x.left - kl + 1);
            var b = x.Slice(x.left - kl, x.right);
            var c = y.Slice(y.left, y.left - kl + 1);
            var d = y.Slice(y.left - kl, y.right);
            var s1 = new Number(result.digits, result.left, result.right + (kr << 1));
            Multiply(a, c, s1);
            var s2 = new Number(result.digits, result.right + (kr << 1) - 1, result.right);
            Multiply(b, d, s2);
            var temp = new byte[(kr + 1) << 2];
            var apb = new Number(temp, kr, 0);
            var cpd = new Number(temp, (kr << 1) + 1, kr + 1);
            var s3 = new Number(temp, ((kr + 1) << 2) - 1, (kr << 1) + 2);
            Number.Add(a, b, apb);
            Number.Add(c, d, cpd);
            Multiply(apb, cpd, s3);
            Number.Subtract(s3, s1, s3);
            Number.Subtract(s3, s2, s3);
            var s4 = new Number(result.digits, result.left, result.right + kr);
            Number.Add(s3, s4, s4);
        }

        private struct Number
        {
            public readonly byte[] digits;
            public readonly int left;
            public readonly int right;

            public Number(byte[] digits, int left, int right)
            {
                if (left < 0)
                    throw new InvalidOperationException();
                if (left >= digits.Length)
                    throw new InvalidOperationException();
                if (right < 0)
                    throw new InvalidOperationException();
                if (right >= digits.Length)
                    throw new InvalidOperationException();
                if (left < right)
                    throw new InvalidOperationException();
                this.digits = digits;
                this.left = left;
                this.right = right;
            }

            public static void MulGradeSchool(Number x, Number y, Number result)
            {
                for (var j = y.right; j <= y.left; j++)
                {
                    byte inc = 0;
                    var resultIndex = result.right + j - y.right;
                    for (var i = x.right; i <= x.left; i++)
                    {
                        var d1 = x.digits[i];
                        var d2 = y.digits[j];
                        var r0 = result.digits[resultIndex] + inc + mulTable0[d1, d2];
                        if (r0 >= 10)
                        {
                            r0 -= 10;
                            inc = 1;
                        }
                        else
                            inc = 0;
                        inc += mulTable1[d1, d2];
                        result.digits[resultIndex] = (byte) r0;
                        resultIndex++;
                    }
                    if (resultIndex <= result.left)
                    {
                        result.digits[resultIndex] += inc;
                        if (result.digits[resultIndex] >= 10)
                        {
                            result.digits[resultIndex + 1]++;
                            result.digits[resultIndex] -= 10;
                        }
                    }
                }
                CheckShit(result);
            }

            private static void CheckShit(Number r)
            {
                //for(var i = r.right; i < r.left; i++)
                //    if(r.digits[i] >= 10)
                //        throw new InvalidOperationException("shit");
            }

            public static void Add(Number x, Number y, Number result)
            {
                if (x.Length < y.Length)
                {
                    var t = x;
                    x = y;
                    y = t;
                }
                byte inc = 0;
                var j = y.right;
                var l = result.right;
                for (var i = x.right; i <= x.left; i++)
                {
                    var r = (byte) (x.digits[i] + inc);
                    if (j <= y.left)
                        r += y.digits[j];
                    if (r >= 10)
                    {
                        inc = 1;
                        r -= 10;
                    }
                    else
                        inc = 0;
                    if (l > result.left)
                    {
                        if (r > 0)
                            throw new InvalidOperationException("shit happens");
                    }
                    else
                        result.digits[l] = r;
                    j++;
                    l++;
                }
                if (inc > 0)
                    result.digits[l] = inc;
                CheckShit(result);
            }

            public static void Subtract(Number x, Number y, Number result)
            {
                sbyte inc = 0;
                var j = y.right;
                var l = result.right;
                for (var i = x.right; i <= x.left; i++)
                {
                    var r = (sbyte) x.digits[i] - inc;
                    if (j <= y.left)
                        r -= y.digits[j];
                    if (r < 0)
                    {
                        inc = 1;
                        r += 10;
                    }
                    else
                        inc = 0;
                    result.digits[l] = (byte) r;
                    j++;
                    l++;
                }
                CheckShit(result);
            }

            public int Length
            {
                get { return left - right + 1; }
            }

            public Number Slice(int newLeft, int newRight)
            {
                return new Number(digits, newLeft, newRight);
            }

            public static Number From(byte[] digits)
            {
                return new Number(digits, digits.Length - 1, 0);
            }
        }
    }
}