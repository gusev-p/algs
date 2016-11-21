namespace Algs.Tasks.Strings
{
    public class SimilarStringsTask
    {
        private readonly byte[] source;
        private readonly uint[,] mappings;
        private int conflictsCount;
        private const int mappingsCount = 11;

        public SimilarStringsTask(string source)
        {
            this.source = ToBytes(source);
            mappings = new uint[mappingsCount, mappingsCount];
        }

        public int GetSimilarSubstringsCount(int l, int r)
        {
            var patternLength = r - l + 1;
            for (var i = 0; i < mappingsCount; i++)
                for (var j = 0; j < mappingsCount; j++)
                    mappings[i, j] = 0;
            conflictsCount = 0;
            for (var i = 0; i < patternLength; i++)
                Add(i, l + i);
            var sampleStart = 0;
            var sampleFinish = r;
            var similarSubstringsCount = 0;
            while (true)
            {
                if (conflictsCount == 0)
                    similarSubstringsCount++;
                sampleFinish++;
                if (sampleFinish == source.Length)
                    break;
                Add(sampleFinish, r);
                Remove(sampleStart, l);
                sampleStart++;
            }
            return similarSubstringsCount;
        }

        private void Add(int sampleIndex, int patternIndex)
        {
            var sampleCharIndex = source[sampleIndex];
            var patternCharIndex = source[patternIndex];
            if (++mappings[sampleCharIndex, patternCharIndex] == 1)
            {
                if (++mappings[sampleCharIndex, mappingsCount - 1] == 2)
                    conflictsCount++;
                if (++mappings[mappingsCount - 1, patternCharIndex] == 2)
                    conflictsCount++;
            }
        }

        private void Remove(int sampleIndex, int patternIndex)
        {
            var sampleCharIndex = source[sampleIndex];
            var patternCharIndex = source[patternIndex];
            if (--mappings[sampleCharIndex, patternCharIndex] == 0)
            {
                if (--mappings[sampleCharIndex, mappingsCount - 1] == 1)
                    conflictsCount--;
                if (--mappings[mappingsCount - 1, patternCharIndex] == 1)
                    conflictsCount--;
            }
        }

        private static byte[] ToBytes(string s)
        {
            var result = new byte[s.Length];
            for (var i = 0; i < s.Length; i++)
                result[i] = (byte) (s[i] - 'a');
            return result;
        }
    }
}