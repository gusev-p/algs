using System;
using Algs.TestUtilities;

namespace Algs.Tasks.Segments
{
    public static class KindergartenAdventures
    {
        public static void TaskMain()
        {
            Input.ReadInt();
            var t = Input.ReadInts();
            var x = FindStartingPoint(t) + 1;
            Console.WriteLine(x);
        }

        private static int FindStartingPoint(int[] t)
        {
            var opened = new int[t.Length];
            for (var i = 0; i < t.Length; i++)
            {
                if (t[i] == t.Length || t[i] == 0)
                    continue;
                var start = i + 1;
                if (start == t.Length)
                    start = 0;
                opened[start]++;
                var finish = i - t[i] + 1;
                if (finish < 0)
                    finish += t.Length;
                opened[finish]--;
            }
            var maxOpenedIndex = -1;
            var maxOpened = int.MinValue;
            var currentOpen = 0;
            for (var i = 0; i < opened.Length; i++)
            {
                currentOpen += opened[i];
                if (currentOpen > maxOpened)
                {
                    maxOpenedIndex = i;
                    maxOpened = currentOpen;
                }
            }
            return maxOpenedIndex;
        }
    }
}