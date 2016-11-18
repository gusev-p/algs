using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Strings
{
    public static class BiggerIsGreater
    {
        public static void TaskMain()
        {
            var t = Input.ReadInt();
            for (var _ = 0; _ < t; _++)
            {
                var w = Console.ReadLine();
                var s = FindMinBigger(w);
                Console.WriteLine(s ?? "no answer");
            }
        }

        private static string FindMinBigger(string w)
        {
            var chars = w.ToCharArray();
            var minIndex = -1;
            for (var i = w.Length - 1; i > 0; i--)
                if (chars[i - 1] < chars[i])
                {
                    minIndex = i - 1;
                    break;
                }
            if (minIndex == -1)
                return null;
            var j = minIndex + 1;
            while (j < chars.Length && chars[j] > chars[minIndex])
                j++;
            var t = chars[minIndex];
            chars[minIndex] = chars[j - 1];
            chars[j - 1] = t;
            var left = minIndex + 1;
            var right = chars.Length - 1;
            while (left < right)
            {
                t = chars[left];
                chars[left] = chars[right];
                chars[right] = t;
                left++;
                right--;
            }
            return new string(chars);
        }
    }
}