namespace Algs.Tasks.GraphAlg
{
    public static class MaxRegionFinder
    {
        public static int GetMaxRegionSize(int[][] grid)
        {
            var result = 0;
            for (var i = 0; i < grid.Length; i++)
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var regionSize = GetMaxRegionSize(grid, i, j);
                    if (regionSize > result)
                        result = regionSize;
                }
            return result;
        }

        private static readonly int[] shifts = {-1, 0, 1};

        private static int GetMaxRegionSize(int[][] grid, int row, int col)
        {
            if (grid[row][col] == 0)
                return 0;
            grid[row][col] = 0;
            var result = 1;
            foreach (var rowShift in shifts)
            {
                var newRow = row + rowShift;
                if (newRow < 0 || newRow == grid.Length)
                    continue;
                foreach (var colShift in shifts)
                {
                    if (rowShift == 0 && colShift == 0)
                        continue;
                    var newCol = col + colShift;
                    if (newCol < 0 || newCol == grid[row].Length)
                        continue;
                    result += GetMaxRegionSize(grid, newRow, newCol);
                }
            }
            return result;
        }
    }
}