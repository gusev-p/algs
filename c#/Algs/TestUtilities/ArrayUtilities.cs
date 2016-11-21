using System;

namespace Algs.TestUtilities
{
    public static class ArrayUtilities
    {
        public static T[] Copy<T>(this T[] array)
        {
            var result = new T[array.Length];
            Array.Copy(array, result, result.Length);
            return result;
        }
    }
}