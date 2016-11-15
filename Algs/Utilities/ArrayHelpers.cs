using System;

namespace Algs.Utilities
{
    public static class ArrayHelpers
    {
        public static T[] Copy<T>(this T[] array)
        {
            var result = new T[array.Length];
            Array.Copy(array, result, result.Length);
            return result;
        }
    }
}