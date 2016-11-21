using System;
using System.Text;
using NUnit.Framework;

namespace Algs.Tasks.Arrays
{
    public static class MatrixRotator
    {
        [TestFixture]
        public class TestFixture
        {
            [Test]
            public void Simple1()
            {
                var m = new[,]
                {
                    {1, 2, 3, 4},
                    {5, 6, 7, 8},
                    {9, 10, 11, 12},
                    {13, 14, 15, 16}
                };
                Console.Out.WriteLine("source:");
                PrintMatrix(m);
                Console.Out.WriteLine("rotated:");
                RotateInplace(m);
                PrintMatrix(m);
            }

            [Test]
            public void Simple2()
            {
                var m = new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6},
                    {7, 8, 9}
                };
                Console.Out.WriteLine("source:");
                PrintMatrix(m);
                Console.Out.WriteLine("rotated:");
                RotateInplace(m);
                PrintMatrix(m);
            }

            private static void PrintMatrix(int[,] m)
            {
                var b = new StringBuilder();
                for (var i = 0; i < m.GetLength(0); i++)
                {
                    for (var j = 0; j < m.GetLength(1); j++)
                        b.AppendFormat("{0,5}", m[i, j]);
                    b.AppendLine();
                }
                Console.Out.WriteLine(b);
            }
        }

        public static void RotateInplace<T>(T[,] squareMatrix)
        {
            var len = squareMatrix.GetLength(0);
            var moves = new Position[4];
            for (var k = 0; k < len/2; k++)
                for (var i = k; i < len - k - 1; i++)
                {
                    moves[0] = new Position(k, i);
                    moves[1] = new Position(i, len - 1 - k);
                    moves[2] = new Position(len - 1 - k, len - 1 - i);
                    moves[3] = new Position(len - 1 - i, k);
                    var last = squareMatrix[moves[3].row, moves[3].col];
                    for (var j = 0; j < moves.Length; j++)
                    {
                        var t = squareMatrix[moves[j].row, moves[j].col];
                        squareMatrix[moves[j].row, moves[j].col] = last;
                        last = t;
                    }
                }
        }

        private struct Position
        {
            public readonly int row;
            public readonly int col;

            public Position(int row, int col)
            {
                this.row = row;
                this.col = col;
            }
        }
    }
}