﻿using System;

namespace Algs.Utilities
{
    public static class Input
    {
        public static int ReadInt()
        {
            return int.Parse(Console.ReadLine());
        }

        public static int[] ReadInts()
        {
            return Array.ConvertAll(ReadStrings(), int.Parse);
        }

        public static long[] ReadLongs()
        {
            return Array.ConvertAll(ReadStrings(), long.Parse);
        }

        public static string[] ReadStrings()
        {
            return Console.ReadLine().Split(' ');
        }
    }
}