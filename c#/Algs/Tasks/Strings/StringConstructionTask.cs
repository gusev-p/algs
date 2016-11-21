namespace Algs.Tasks.Strings
{
    public class StringConstructionTask
    {
        private readonly string source;

        public StringConstructionTask(string source)
        {
            this.source = source;
        }

        public int Execute()
        {
            var viewedChars = new bool[26];
            var uniqueCharsCount = 0;
            foreach (var c in source)
            {
                if (!viewedChars[c - 'a'])
                {
                    viewedChars[c - 'a'] = true;
                    uniqueCharsCount++;
                }
            }
            return uniqueCharsCount;
        }
    }
}