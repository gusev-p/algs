using System;
using Algs.Utilities;

namespace Algs.Tasks.Arrays
{
    public static class TheBombermanGame
    {
        public static void TaskMain()
        {
            var line = Input.ReadStrings();
            var r = int.Parse(line[0]);
            var c = int.Parse(line[1]);
            var n = long.Parse(line[2]);
            var grid = new char[r][];
            for (var i = 0; i < r; i++)
            {
                grid[i] = Input.ReadChars();
                if (grid[i].Length != c)
                    throw new InvalidOperationException("assertion failure");
            }
            DetermineState(grid, n);
            for (var i = 0; i < r; i++)
                Console.WriteLine(new string(grid[i]));
        }

        private static void DetermineState(char[][] grid, long n)
        {
            if (n == 1)
                return;
            if (n%2 == 0)
            {
                PlantBombsInAllCells(grid);
                return;
            }
            MakeSingleMove(grid);
            if ((n/2 - 1)%2 == 1)
                MakeSingleMove(grid);
        }

        private static void MakeSingleMove(char[][] grid)
        {
            for (var i = 0; i < grid.Length; i++)
            {
                var row = grid[i];
                for (var j = 0; j < row.Length; j++)
                {
                    if (row[j] != 'O')
                        continue;
                    foreach (var m in moves)
                    {
                        var siblingRow = i + m.row;
                        if (siblingRow < 0 || siblingRow == grid.Length)
                            continue;
                        var siblingCol = j + m.col;
                        if (siblingCol < 0 || siblingCol == grid[siblingRow].Length)
                            continue;
                        if (grid[siblingRow][siblingCol] == '.')
                            grid[siblingRow][siblingCol] = 'T';
                    }
                }
            }
            foreach (var row in grid)
                for (var j = 0; j < row.Length; j++)
                    row[j] = row[j] == 'T' || row[j] == 'O' ? '.' : 'O';
        }

        private static void PlantBombsInAllCells(char[][] grid)
        {
            foreach (var row in grid)
                for (var j = 0; j < row.Length; j++)
                    row[j] = 'O';
        }

        private static readonly Move[] moves =
        {
            new Move(0, 1),
            new Move(0, -1),
            new Move(1, 0),
            new Move(-1, 0)
        };

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