using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Algs.Core;
using Algs.TestUtilities;
using NUnit.Framework;

namespace Algs.Tests.Core
{
    [TestFixture]
    public class RBTreeTest
    {
        [Test]
        public void SimpleAddGet()
        {
            var rbTree = new RBTree();
            rbTree.Add(1, 2);
            int value;
            Assert.That(rbTree.TryGetValue(1, out value));
            Assert.That(value, Is.EqualTo(2));
            Assert.That(rbTree.TryGetValue(2, out value), Is.False);
            var exception = Assert.Throws<InvalidOperationException>(() => rbTree.Add(1, 3));
            Assert.That(exception.Message, Is.EqualTo("key [1] already exist"));
        }

        [Test]
        public void SimpleAddGetDelete()
        {
            var rbTree = new RBTree();
            rbTree[1] = 2;
            Assert.That(rbTree[1], Is.EqualTo(2));
            Assert.That(rbTree.Count, Is.EqualTo(1));
            rbTree[1] = 3;
            Assert.That(rbTree[1], Is.EqualTo(3));
            Assert.That(rbTree.Count, Is.EqualTo(1));
            rbTree.Remove(1);
            Assert.That(rbTree.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestHeightBound()
        {
            var random = new Random();
            var rbTree = new RBTree();
            const int limit = 100000;
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int existingValue;
                if (rbTree.TryGetValue(key, out existingValue))
                    rbTree.Remove(key);
                else
                    rbTree.Add(key, 42);
                var height = rbTree.GetHeight();
                var heightUpperBound = 2*Math.Log(rbTree.Count + 1, 2);
                if (height > heightUpperBound)
                {
                    const string messageFormat = "iteration [{0}], height invariant violation, " +
                                                 "height [{1}], upper bound from invariant [{2}]";
                    Assert.Fail(messageFormat, i, height, heightUpperBound);
                }
            }
        }

        [Test]
        public void Bug()
        {
            var numbers = new[] {1, 2, 0, 3};
            var rbTree = new RBTree();
            foreach (var k in numbers)
            {
                rbTree.Check();
                rbTree.TryAdd(k, 1);
                rbTree.Check();
            }
            Assert.That(rbTree.TryRemove(0));
            Assert.That(rbTree.TryAdd(2, 1), Is.False);
        }

        private static void Shuffle<T>(T[] items)
        {
            var random = new Random();
            for (var i = 1; i < items.Length; i++)
            {
                var r = random.Next(i + 1);
                var t = items[r];
                items[r] = items[i];
                items[i] = t;
            }
        }

        private static int[] GetRandomArray(int length)
        {
            var result = new int[length];
            for (var i = 0; i < length; i++)
                result[i] = i;
            Shuffle(result);
            return result;
        }

        [Test]
        public void RevealBug()
        {
            const string fileName = @"C:\sources\Algs\c#\Algs\bin\Debug\log";
            var random = new Random();
            for (var i = 7; i <= 100; i++)
                for (var k = 0; k < 1000; k++)
                {
                    var numbers = GetRandomArray(i);
                    var numbersCopy = numbers.Copy();
                    File.AppendAllText(fileName,
                        "\r\n[" + string.Join(",", numbersCopy) + "], removes: ");
                    var rbTree = new RBTree();
                    foreach (var n in numbers)
                        rbTree.Add(n, 0);
                    var len = numbers.Length;
                    var removeHistory = new List<int>();
                    try
                    {
                        while (len > 0)
                        {
                            var indexToRemove = random.Next(len);
                            var key = numbers[indexToRemove];
                            removeHistory.Add(key);
                            rbTree.Remove(key);
                            File.AppendAllText(fileName, "," + key);
                            rbTree.Check();
                            numbers[indexToRemove] = numbers[len - 1];
                            len--;
                            foreach (var x in removeHistory)
                                Assert.That(rbTree.TryRemove(x), Is.False);
                            for (var j = 0; j < len; j++)
                                if (rbTree.TryAdd(numbers[j], 22))
                                    Assert.Fail("can add key " + numbers[j]);
                        }
                    }
                    catch (Exception e)
                    {
                        const string messageFormat = "SHIT, numbers [{0}], remove history [{1}]";
                        throw new InvalidOperationException(string.Format(messageFormat,
                            string.Join(",", numbersCopy),
                            string.Join(",", removeHistory)), e);
                    }
                }
        }

        [Test]
        public void Correctnetss_RandomAddDelete()
        {
            var random = new Random();
            var etalon = new Dictionary<int, int>();
            var rbTree = new RBTree();
            const int limit = 10000000;
            var matchedFinds = 0;
            for (var i = 0; i < limit; i++)
            {
                try
                {
                    var key = random.Next(limit);
                    int existingValue;
                    if (rbTree.TryGetValue(key, out existingValue))
                    {
                        var expectedValue = etalon[key];
                        if (existingValue != expectedValue)
                        {
                            const string messageFormat = "iteration [{0}], for key [{1}] existing value [{2}] " +
                                                         "not matched with expected value [{3}]";
                            Assert.Fail(messageFormat, i, key, existingValue, expectedValue);
                        }
                        matchedFinds++;
                        rbTree.Remove(key);
                        etalon.Remove(key);
                    }
                    else
                    {
                        var value = random.Next(limit);
                        rbTree.Add(key, value);
                        etalon.Add(key, value);
                    }
                }
                catch (Exception e)
                {
                    const string messageFormat = "bug at [{0}]";
                    throw new InvalidOperationException(string.Format(messageFormat, i), e);
                }
            }
            Console.Out.WriteLine("ok, matched finds [{0}]", matchedFinds);
        }


        //SortedDictionary: [89043] millis
        //RBTree: elapsed [43872]
        [Test]
        public void LoadTest_AddGetRemove_UniformRandom()
        {
            var random = new Random();
            //var rbTree = new SortedDictionary<int, int>();
            var rbTree = new RBTree();
            const int limit = 10000000;
            var matchedFinds = 0;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int _;
                if (rbTree.TryGetValue(key, out _))
                {
                    rbTree.Remove(key);
                    matchedFinds++;
                }
                else
                    rbTree.Add(key, 42);
            }
            stopwatch.Stop();
            Console.Out.WriteLine("ok, matched finds [{0}], elapsed [{1}] millis",
                matchedFinds, stopwatch.ElapsedMilliseconds);
        }
    }
}