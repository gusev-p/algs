using System;
using System.Collections.Generic;
using System.Diagnostics;
using Algs.Core;
using NUnit.Framework;

namespace Algs.Tests.Core
{
    [TestFixture]
    public class RBTreeNoParentsTest
    {
        [Test]
        public void SimpleAddGet()
        {
            var rbTree = new RBTreeNoParent();
            Assert.That(rbTree.TryAdd(1, 2));
            int value;
            Assert.That(rbTree.TryGetValue(1, out value));
            Assert.That(value, Is.EqualTo(2));
            Assert.That(rbTree.TryGetValue(2, out value), Is.False);
            Assert.That(rbTree.TryAdd(1, 3), Is.False);
        }

        [Test]
        public void MoreAddGet()
        {
            var numbers = new[] {10, 6, 2, 8, 9, 3, 4, 13, 12};
            var rbTree = new RBTreeNoParent();
            for (var i = 0; i < numbers.Length; i++)
                Assert.That(rbTree.TryAdd(numbers[i], i));
            for (var i = 0; i < numbers.Length; i++)
            {
                int value;
                var key = numbers[i];
                Assert.That(rbTree.TryGetValue(key, out value),
                    () => string.Format("key [{0}] not found", key));
                Assert.That(value, Is.EqualTo(i));
            }
        }

        [Test]
        public void ManyAddGetPrevious()
        {
            var rbTree = new RBTreeNoParent();
            var numbers = new[] {89, 96, 37, 51, 40, 16, 50, 79, 38, 72};
            for (var i = 0; i < numbers.Length; i++)
            {
                Assert.That(rbTree.TryAdd(numbers[i], i));
                for (var j = 0; j <= i; j++)
                {
                    int value;
                    Assert.That(rbTree.TryGetValue(numbers[j], out value));
                    Assert.That(value, Is.EqualTo(j));
                }
            }
        }

        [Test]
        public void CorrectnessAddGetUniformRandom()
        {
            var random = new Random();
            var etalon = new Dictionary<int, int>();
            var rbTree = new RBTreeNoParent();
            var history = new List<int>();
            const int limit = 100000;
            var matchedFinds = 0;
            try
            {
                for (var i = 0; i < limit; i++)
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
                    }
                    else
                    {
                        var value = random.Next(limit);
                        Assert.That(rbTree.TryAdd(key, value));
                        etalon.Add(key, value);
                        history.Add(key);
                    }
                }
            }
            catch (Exception e)
            {
                const string messageFormat = "error, history [{0}]";
                throw new InvalidOperationException(string.Format(messageFormat, string.Join(",", history)), e);
            }
            Console.Out.WriteLine("ok, matched finds [{0}]", matchedFinds);
        }

        [Test]
        public void TestHeightBound()
        {
            var random = new Random();
            var rbTree = new RBTreeNoParent();
            const int limit = 100000;
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int existingValue;
                if (rbTree.TryGetValue(key, out existingValue))
                    continue;
                rbTree.Add(key, 1);
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

        //SortedDictionary: took [41144] millis
        //RBTreeNoParent: took [37171] millis
        //RBTree: took [34307] millis
        [Test]
        public void LoadTestAddGetUniformRandom()
        {
            var random = new Random();
            var rbTree = new SortedDictionary<int, int>();
            //var rbTree = new RBTreeNoParent();
            //var rbTree = new RBTree();
            const int limit = 10000000;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int existingValue;
                if (!rbTree.TryGetValue(key, out existingValue))
                {
                    var value = random.Next(limit);
                    rbTree.Add(key, value);
                }
            }
            stopwatch.Stop();
            Console.Out.WriteLine("took [{0}] millis", stopwatch.ElapsedMilliseconds);
        }
    }
}