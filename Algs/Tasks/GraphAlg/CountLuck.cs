using System;

namespace Algs.Tasks.GraphAlg
{
    public static class CountLuck
    {
        public static void TaskMain()
        {
            var t = int.Parse(Console.ReadLine());
            for (var _ = 0; _ < t; _++)
            {
                var line = Console.ReadLine().Split(' ');
                var n = int.Parse(line[0]);
                var forest = new char[n][];
                for (var i = 0; i < n; i++)
                    forest[i] = Console.ReadLine().ToCharArray();
                var k = int.Parse(Console.ReadLine());
                var actualWaves = GetWandWavesCount(forest);
                Console.WriteLine(k == actualWaves ? "Impressed" : "Oops!");
            }
        }

        private static int GetWandWavesCount(char[][] forest)
        {
            for (var i = 0; i < forest.Length; i++)
                for (var j = 0; j < forest[i].Length; j++)
                    if (forest[i][j] == 'M')
                        return GetWandWavesCount(forest, i, j);
            throw new InvalidOperationException("assertion failure");
        }

        private static readonly Move[] moves =
        {
            new Move(-1, 0),
            new Move(1, 0),
            new Move(0, 1),
            new Move(0, -1)
        };

        private static int GetWandWavesCount(char[][] forest, int row, int col)
        {
            if (forest[row][col] == '*')
                return 0;
            forest[row][col] = 'V';
            var possibleDirectionsCount = 0;
            var wayFound = false;
            var result = -1;
            foreach (var m in moves)
            {
                var newRow = row + m.row;
                if (newRow < 0 || newRow == forest.Length)
                    continue;
                var newCol = col + m.col;
                if (newCol < 0 || newCol == forest[newRow].Length)
                    continue;
                var c = forest[newRow][newCol];
                if (c == 'X' || c == 'V')
                    continue;
                if (c == '.' || c == '*')
                {
                    possibleDirectionsCount++;
                    if (!wayFound)
                    {
                        result = GetWandWavesCount(forest, newRow, newCol);
                        if (result >= 0)
                            wayFound = true;
                    }
                    continue;
                }
                throw new InvalidOperationException("assertion failure");
            }
            if (result == -1)
                return -1;
            if (possibleDirectionsCount > 1)
                result++;
            return result;
        }

        private class Move
        {
            public readonly int row;
            public readonly int col;

            public Move(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }
    }
}