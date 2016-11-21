using Algs.Tasks.Heaps;
using NUnit.Framework;

namespace Algs.Tests.Tasks
{
    [TestFixture]
    public class MedianCounterTest
    {
        [Test]
        public void Simple()
        {
            var testInput = new[] {12, 4, 5, 3, 8, 7};
            var testOutput = new[] {12.0, 8.0, 5.0, 4.5, 5.0, 6.0};
            var counter = new MedianCounter(testInput.Length);
            for (var i = 0; i < testInput.Length; i++)
            {
                counter.Add(testInput[i]);
                Assert.That(counter.Median, Is.EqualTo(testOutput[i]));
            }
        }
    }
}