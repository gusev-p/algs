using System.Collections.Generic;

namespace Algs.Tasks.Stacks
{
    public static class BracketsChecker
    {
        public static bool IsBalanced(string s)
        {
            var stack = new Stack<char>();
            foreach (var c in s)
            {
                if (c == '[' || c == '{' || c == '(')
                    stack.Push(c);
                else
                {
                    if (stack.Count == 0)
                        return false;
                    var openBracket = stack.Pop();
                    switch (openBracket)
                    {
                        case '[':
                            if (c != ']')
                                return false;
                            break;
                        case '{':
                            if (c != '}')
                                return false;
                            break;
                        case '(':
                            if (c != ')')
                                return false;
                            break;
                    }
                }
            }
            return stack.Count == 0;
        }
    }
}