using System;

namespace Algs.TestUtilities
{
    public static class Input
    {
        public static int ReadInt()
        {
            return int.Parse(Console.ReadLine());
        }

        public static int[] ReadInts()
        {
            return Array.ConvertAll(ReadStrings(), s => int.Parse(s.Trim()));
        }

        public static long[] ReadLongs()
        {
            return Array.ConvertAll(ReadStrings(), long.Parse);
        }

        public static string[] ReadStrings()
        {
            return Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static char[] ReadChars()
        {
            return Console.ReadLine().ToCharArray();
        }
    }
}