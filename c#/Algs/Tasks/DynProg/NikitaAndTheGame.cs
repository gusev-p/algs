namespace Algs.Tasks.DynProg
{
    public class NikitaAndTheGame
    {
        private readonly long[] array;
        private readonly long[] leftIncremental;
        private readonly long[] rightIncremental;

        public NikitaAndTheGame(long[] array)
        {
            this.array = array;
            leftIncremental = new long[array.Length];
            rightIncremental = new long[array.Length];
            long sumLeft = 0;
            for (var i = 0; i < array.Length; i++)
            {
                sumLeft += array[i];
                leftIncremental[i] = sumLeft;
            }
            long sumRight = 0;
            for (var i = array.Length - 1; i >= 0; i--)
            {
                sumRight += array[i];
                rightIncremental[i] = sumRight;
            }
        }

        public int Solve()
        {
            return CountMoves(0, leftIncremental.Length - 1);
        }

        private int CountMoves(int left, int right)
        {
            if (left >= right)
                return 0;
            var l = left;
            var r = right;
            long sumLeft;
            long sumRight;
            var sumLeftDiff = left == 0 ? 0 : leftIncremental[left - 1];
            var sumRighDiff = right == rightIncremental.Length - 1 ? 0 : rightIncremental[right + 1];
            while (l < r)
            {
                var mid = l + (r - l)/2;
                sumLeft = leftIncremental[mid] - sumLeftDiff;
                sumRight = rightIncremental[mid] - sumRighDiff - array[mid];
                if (sumLeft >= sumRight)
                    r = mid;
                else
                    l = mid + 1;
            }
            sumLeft = leftIncremental[l] - sumLeftDiff;
            sumRight = rightIncremental[l] - sumRighDiff - array[l];
            if (sumLeft != sumRight)
                return 0;
            var leftMoves = CountMoves(left, l);
            var rightMoves = CountMoves(l + 1, right);
            var max = leftMoves > rightMoves ? leftMoves : rightMoves;
            return max + 1;
        }
    }
}