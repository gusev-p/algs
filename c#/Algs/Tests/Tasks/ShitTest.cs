using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Algs.Tasks.Arrays;
using Algs.Tasks.GraphAlg;
using Algs.Tasks.Numbers;
using Algs.Tasks.Sorting;
using NUnit.Framework;

namespace Algs.Tests.Tasks
{
    [TestFixture]
    public class ShitTest
    {
        [Test]
        public void Shit2()
        {
            Console.Out.WriteLine(123.ToString()[0]);
            
        }

        [Test]
        public void xxx()
        {
            var m = new int[16, 16];
            long counter = 0;
            var s = Stopwatch.StartNew();
            var limit = (long) Math.Pow(2, 24);
            while (counter < limit)
            {
                for (var i = 0; i < m.GetLength(0); i++)
                    for (var j = 0; j < m.GetLength(1); j++)
                    {
                        m[i, j]++;
                        counter++;
                    }
            }
            s.Stop();
            Console.Out.WriteLine("elapsed [{0}] millis", s.ElapsedMilliseconds);
        }

        [Test]
        public void Zzz66()
        {
            var s1 = "dkhc";
            var s2 = "ckhd";
            var r = StringComparer.OrdinalIgnoreCase.Compare(s1, s2);
            Console.Out.WriteLine(r < 0 ? "first smaller" : "first not smaller");
        }

        [Test]
        public void Zzz211112()
        {
            const int maxCycleLength = 2000;
            for (var i = 4; i < maxCycleLength; i++)
            {
                var g = CreateCycleOfLength(i);
                var expectedWeight = (i - 1)*i/2*100;
                for (var j = 0; j < g.NodesCount; j++)
                {
                    var mstWeight = PrimMSTSpecialSubtree.PrimMSTAlgorithm.GetMSTWeight(g, j);
                    if (mstWeight != expectedWeight)
                    {
                        Console.Out.WriteLine("shit, start [{0}], size [{1}], expected [{2}], actual [{3}]",
                            j, i, expectedWeight, mstWeight);
                        Assert.Fail();
                    }
                }
                File.WriteAllText(
                    @"C:\sources\knopka.statistics\_Modules\ConsoleApplication1\ConsoleApplication1\bin\Debug\hujResult",
                    i.ToString());
            }
        }

        private static PrimMSTSpecialSubtree.OutgoingWeightedGraph CreateCycleOfLength(int length)
        {
            var g = new PrimMSTSpecialSubtree.OutgoingWeightedGraph(length);
            for (var j = 0; j < g.NodesCount - 1; j++)
                g.AddBilateralEdge(j, j + 1, (j + 1)*100);
            g.AddBilateralEdge(g.NodesCount - 1, 0, (g.NodesCount - 1)*100);
            return g;
        }

        [Test]
        public void Zzz22()
        {
            Console.Out.WriteLine(long.MaxValue);
            Console.Out.WriteLine(int.MaxValue);
            Console.Out.WriteLine(Math.Pow(10, 9));
            checked
            {
                Console.Out.WriteLine(((long) Math.Pow(10, 9))*16384);
            }
        }

        [Test]
        public void Zzz()
        {
            const string path =
                @"C:\sources\knopka.statistics\_Modules\ConsoleApplication1\ConsoleApplication1\bin\Debug\huj.txt";
            var strings = File.ReadAllLines(path);
            var numbers = Array.ConvertAll(strings, int.Parse);
            var t = Stopwatch.StartNew();
            QuickSorter2.QuickSort(numbers);
            t.Stop();
            Console.Out.WriteLine("comparisons [{0}], took [{1}] millis",
                QuickSorter2.fuckingComparisonsCount, t.ElapsedMilliseconds);
        }

        [Test]
        public void Test()
        {
            var b = new StringBuilder();
            for (var i = 0; i <= 9; i++)
            {
                b.Append("{");
                for (var j = 0; j <= 9; j++)
                {
                    if (j != 0)
                        b.Append(", ");
                    b.Append(i*j/10);
                }
                b.Append("},");
                b.AppendLine();
            }
            Console.Out.WriteLine(b);
        }

        [Test]
        public void Test22()
        {
            var x = new byte[] {1, 2, 3, 4};
            var y = new byte[] {5, 6, 7, 8};
            //var result = KaratsubaMultiplier.Multiply(x, y);
            var result = KaratsubaMultiplier.Multiply("4321", "8765");
            Console.Out.WriteLine(result);
        }

        [Test]
        public void Test222()
        {
            var random = new Random();
            for (var i = 0; i < 10000; i++)
            {
                var len = random.Next(100);
                var array = new int[len];
                for (var j = 0; j < array.Length; j++)
                    array[j] = random.Next(10000) - 5000;

                var testResult = MaxSubArrayFinder.FindMaxSubArray(array);
                var maxLeft = -1;
                var maxRight = -1;
                var maxSum = long.MinValue;
                for (var j = 0; j < array.Length; j++)
                {
                    if (array[j] == 0)
                        continue;
                    for (var k = j; k < array.Length; k++)
                    {
                        if (array[k] == 0)
                            continue;
                        long sum = 0;
                        for (var l = j; l <= k; l++)
                            sum += array[l];
                        if (sum > maxSum)
                        {
                            maxSum = sum;
                            maxLeft = j;
                            maxRight = k;
                        }
                    }
                }
                if (testResult.Item1 >= 0)
                {
                    if (testResult.Item1 != maxLeft || testResult.Item2 != maxRight)
                    {
                        Console.Out.WriteLine("array");
                        Console.Out.WriteLine(string.Join(" ", array));
                        Console.Out.WriteLine("expected [{0},{1}]", maxLeft, maxRight);
                        Console.Out.WriteLine("actual [{0},{1}]", testResult.Item1, testResult.Item2);
                        Assert.Fail();
                    }
                }
            }
        }

        [Test]
        public void Test33()
        {
            var s1 = "3141592653589793238462643383279502884197169399375105820974944592";
            var s2 = "2718281828459045235360287471352662497757247093699959574966967627";
            var s = Stopwatch.StartNew();
            var result = KaratsubaMultiplier.Multiply(s1, s2);
            s.Stop();
            Console.Out.WriteLine("my took {0} millis", s.ElapsedMilliseconds);
            var i = 0;
            while (result[i] == '0')
                i++;
            if (i > 0)
                result = result.Substring(i, result.Length - i);
            var n1 = BigInteger.Parse(s1);
            var n2 = BigInteger.Parse(s2);
            s = Stopwatch.StartNew();
            var etalon = n1*n2;
            s.Stop();
            Console.Out.WriteLine("their took {0} millis", s.ElapsedMilliseconds);
            Assert.That(result, Is.EqualTo(etalon.ToString()));
            Console.Out.WriteLine(result);
        }
    }
}