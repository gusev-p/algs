using System.Diagnostics;
using System.Text;

namespace Algs.Tasks.Strings
{
    public class MakePalindromTask
    {
        private readonly string number;
        private readonly int allowedModificationsCount;

        public MakePalindromTask(string number, int allowedModificationsCount)
        {
            this.number = number;
            this.allowedModificationsCount = allowedModificationsCount;
        }

        public string Execute()
        {
            Debugger.Launch();
            var result = new StringBuilder(number);
            var left = 0;
            var right = result.Length - 1;
            var diffCount = 0;
            while (left < right)
            {
                if (result[left] != result[right])
                    diffCount++;
                left++;
                right--;
            }
            if (diffCount > allowedModificationsCount)
                return "-1";
            left = 0;
            right = result.Length - 1;
            var remainingModifications = allowedModificationsCount;
            var remainingDiffCount = diffCount;
            while (left < right)
            {
                var eq = result[left] == result[right];
                var overrun = result[left] != '9' && result[right] != '9' &&
                              remainingModifications >= 2 &&
                              remainingModifications - 2 >= remainingDiffCount - (eq ? 0 : 1);
                if (overrun)
                {
                    result[left] = '9';
                    result[right] = '9';
                    remainingModifications -= 2;
                    if (!eq)
                        remainingDiffCount--;
                }
                else if (!eq)
                {
                    if (result[left] > result[right])
                        result[right] = result[left];
                    else
                        result[left] = result[right];
                    remainingModifications--;
                    remainingDiffCount--;
                }
                left++;
                right--;
            }
            if (remainingModifications > 0 && (result.Length & 1) != 0)
                result[result.Length >> 1] = '9';
            return result.ToString();
        }
    }
}