using System;
using System.Diagnostics;
using Algs.Tasks.Rol;
using Algs.TestUtilities;
using NUnit.Framework;

namespace Algs.Tests.Tasks
{
    [TestFixture]
    public class RolTest
    {
        [Test]
        public void Test()
        {
            const int testCount = 100;
            const int maxN = 100000;
            const int maxValue = 100000;

            var random = new Random();
            for (var i = 0; i < testCount; i++)
            {
                var n = random.Next(maxN) + 1;
                var d = random.Next(n) + 1;
                var a = new int[n];
                random.NextInts(a, maxValue);

                var inplaceResult = a.Copy();
                var s = Stopwatch.StartNew();
                RolInplace.Execute(inplaceResult, n, d);
                s.Stop();
                var inplaceMillis = s.ElapsedMilliseconds;

                s = Stopwatch.StartNew();
                var simpleResult = RolSimple.Execute(a, n, d);
                s.Stop();
                var simpleMillis = s.ElapsedMilliseconds;
                Assert.That(inplaceResult, Is.EqualTo(simpleResult));

                Console.Out.WriteLine("done {0}, inplace millis {1}, simple millis {2}",
                    i, inplaceMillis, simpleMillis);
            }
        }
    }
}