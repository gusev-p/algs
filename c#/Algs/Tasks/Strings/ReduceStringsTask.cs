namespace Algs.Tasks.Strings
{
    //public static class ReduceStringsTask
    //{
    //    public static int ReduceStrings2(string s)
    //    {
    //        return ReduceStrings2(s, 0, s.Length - 1).count;
    //    }

    //    public static CharWithCount ReduceStrings2(string s, int start, int finish)
    //    {
    //        if (start > finish)
    //            throw new InvalidOperationException("shit22");
    //        if(start == finish)
    //            return new CharWithCount
    //            {
    //                count = 1,
    //                c = s[start]
    //            };
    //        if (finish - start == 1)
    //            return s[start] == s[finish]
    //                ? new CharWithCount {c = s[start], count = 2}
    //                : new CharWithCount
    //                {
    //                    count = 1,
    //                    c = ReduceChars(s[start], s[finish])
    //                };
    //        var mid = start + (finish - start)/2;
    //        var left1 = ReduceStrings2(s, start, mid);
    //        var right1 = ReduceStrings2(s, mid + 1, finish);
    //        var result1 = left1.c == right1.c
    //            ?new CharWithCount{count = left1.count + right1.count}
    //            : 
    //        var left2 = ReduceStrings2(s, start, mid - 1);
    //        var right2 = ReduceStrings2(s, mid, finish);

    //    }

    //    private struct CharWithCount
    //    {
    //        public char c;
    //        public int count;
    //    }

    //    public static int ReduceString1(string a)
    //    {
    //        if (a.Length <= 1)
    //            return a.Length;
    //        var start = 0;
    //        var i = 1;
    //        var p = a[0];
    //        while (i < a.Length)
    //        {
    //            var c = a[i];
    //            if (c != p)
    //            {
    //                for (var j = 0; j < i - start; j++)
    //                    p = ReduceChars(c, p);
    //                start = i;
    //            }
    //            i++;
    //        }
    //        return a.Length - start;
    //    }

    //    private static char ReduceChars(char c1, char c2)
    //    {
    //        if ((c1 == 'a' && c2 == 'b') || (c1 == 'b' && c2 == 'a'))
    //            return 'c';
    //        if ((c1 == 'a' && c2 == 'c') || (c1 == 'c' && c2 == 'a'))
    //            return 'b';
    //        if ((c1 == 'b' && c2 == 'c') || (c1 == 'c' && c2 == 'b'))
    //            return 'a';
    //        throw new InvalidOperationException("shit");
    //    }
    //}
}