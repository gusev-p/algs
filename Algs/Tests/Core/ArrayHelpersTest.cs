using System;
using System.Collections.Generic;
using Algs.Core;
using Algs.TestUtilities;
using NUnit.Framework;

namespace Algs.Tests.Core
{
    [TestFixture]
    public class ArrayHelpersTest
    {
        [Test]
        public void RandomBinarySearch()
        {
            var random = new Random();
            var moreThanOneCount = 0;
            for (var i = 0; i < 10000; i++)
            {
                var array = new int[random.Next(1000) + 1];
                random.NextInts(array, 10000);
                Array.Sort(array);
                var index = random.Next(array.Length);
                var v = array[index];
                var left = index;
                while (left > 0 && array[left - 1] == v)
                    left--;
                var right = index;
                while (right < array.Length - 1 && array[right + 1] == v)
                    right++;
                var testLeft = array.BinarySearch(v, Comparer<int>.Default, Occurence.First);
                Assert.That(testLeft, Is.EqualTo(left));
                var testRight = array.BinarySearch(v, Comparer<int>.Default, Occurence.Last);
                Assert.That(testRight, Is.EqualTo(right));
                if (right - left > 0)
                    moreThanOneCount++;
            }
            Assert.That(moreThanOneCount, Is.GreaterThan(0));
        }
    }
}