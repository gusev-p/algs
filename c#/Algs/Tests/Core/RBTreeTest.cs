using System;
using System.Collections.Generic;
using System.Diagnostics;
using Algs.Core;
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
        public void TestHeightBound()
        {
            var random = new Random();
            var hashtable = new Dictionary<int, int>();
            var rbTree = new RBTree();
            const int limit = 100000;
            var matchedFinds = 0;
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int existingValue;
                if (rbTree.TryGetValue(key, out existingValue))
                {
                    var expectedValue = hashtable[key];
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
                    rbTree.Add(key, value);
                    hashtable.Add(key, value);
                }
                var height = rbTree.GetHeight();
                var heightUpperBound = 2*Math.Log(rbTree.Count + 1, 2);
                if (height > heightUpperBound)
                {
                    const string messageFormat = "iteration [{0}], height invariant violation, " +
                                                 "height [{1}], upper bound from invariant [{2}]";
                    Assert.Fail(messageFormat, i, height, heightUpperBound);
                }
            }
            Assert.That(matchedFinds, Is.GreaterThan(0));
        }

        [Test]
        public void LoadTestAddGetUniformRandom()
        {
            var random = new Random();
            var hashtable = new Dictionary<int, int>();
            var rbTree = new SortedDictionary<int, int>();
            //var rbTree = new RBTree();
            const int limit = 10000000;
            var matchedFinds = 0;
            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < limit; i++)
            {
                var key = random.Next(limit);
                int existingValue;
                if (rbTree.TryGetValue(key, out existingValue))
                {
                    var expectedValue = hashtable[key];
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
                    rbTree.Add(key, value);
                    hashtable.Add(key, value);
                }
                //if (i%100000 == 0)
                //    File.AppendAllText(@"C:\sources\Algs\c#\Algs\bin\Debug\qq",
                //        string.Format("{0}%\r\n", (double) i/limit*100));
            }
            stopwatch.Stop();
            Console.Out.WriteLine("ok, matched finds [{0}], elapsed [{1}] millis",
                matchedFinds, stopwatch.ElapsedMilliseconds);
        }
    }
}