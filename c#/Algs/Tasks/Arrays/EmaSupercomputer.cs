using System;
using System.Collections.Generic;
using Algs.TestUtilities;

namespace Algs.Tasks.Arrays
{
    public static class EmaSupercomputer
    {
        public static void TaskMain()
        {
            var line1 = Console.ReadLine().Split(' ');
            var n = int.Parse(line1[0]);
            var grid = new char[n][];
            for (var i = 0; i < n; i++)
                grid[i] = Input.ReadChars();
            var pluses = GetAllPluses(grid);
            pluses.Sort(Plus.CompareByRadius);
            var maxProduct = GetMaxPlusProduct(pluses);
            Console.WriteLine(maxProduct);
        }

        private static int GetMaxPlusProduct(List<Plus> sortedPluses)
        {
            var maxProduct = -1;
            for (var i = 0; i < sortedPluses.Count; i++)
            {
                var p1 = sortedPluses[i];
                var a1 = p1.Area();
                for (var j = i + 1; j < sortedPluses.Count; j++)
                {
                    var p2 = sortedPluses[j];
                    if (p1.Intersects(p2))
                        continue;
                    var product = a1*p2.Area();
                    if (product > maxProduct)
                        maxProduct = product;
                }
            }
            return maxProduct;
        }

        private class Plus
        {
            private readonly int row;
            private readonly int col;
            private readonly int radius;

            public Plus(int row, int col, int radius)
            {
                this.row = row;
                this.col = col;
                this.radius = radius;
            }

            public int Area()
            {
                return radius + (radius - 1)*3;
            }

            public static int CompareByRadius(Plus p1, Plus p2)
            {
                return p1.radius.CompareTo(p2.radius);
            }

            public bool Intersects(Plus other)
            {
                var dRows = Math.Abs(other.row - row);
                var dCols = Math.Abs(other.col - col);
                if (dRows == 0)
                    return dCols < radius + other.radius + 1;
                if (dCols == 0)
                    return dRows < radius + other.radius + 1;
                return dCols < radius && dRows < other.radius ||
                       dRows < radius && dCols < other.radius;
            }
        }

        private static List<Plus> GetAllPluses(char[][] grid)
        {
            var result = new List<Plus>();
            for (var i = 0; i < grid.Length; i++)
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 'B')
                        continue;
                    result.Add(new Plus(i, j, 1));
                    var k = 1;
                    while (true)
                    {
                        var directionIndex = 0;
                        while (directionIndex < directions.Length)
                        {
                            var direction = directions[directionIndex];
                            var newRow = i + k*direction.row;
                            if (newRow < 0 || newRow >= grid.Length)
                                break;
                            var newCol = j + k*direction.col;
                            if (newCol < 0 || newCol >= grid[i].Length)
                                break;
                            if (grid[newRow][newCol] == 'B')
                                break;
                            directionIndex++;
                        }
                        if (directionIndex != directions.Length)
                            break;
                        result.Add(new Plus(i, j, k + 1));
                        k++;
                    }
                }
            return result;
        }

        private static readonly Direction[] directions =
        {
            new Direction(0, 1),
            new Direction(0, -1),
            new Direction(1, 0),
            new Direction(-1, 0)
        };

        private class Direction
        {
            public readonly int row;
            public readonly int col;

            public Direction(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }
    }
}