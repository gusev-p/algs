using System;

namespace Algs.Tasks.Strings
{
    public static class AnagramFinder
    {
        public static int GetDeletionsCount(string a, string b)
        {
            var charTable = new int[26];
            foreach (var t in a)
                charTable[t - 'a']++;
            foreach (var t in b)
                charTable[t - 'a']--;
            var result = 0;
            foreach (var i in charTable)
                result += Math.Abs(i);
            return result;
        }
    }
}