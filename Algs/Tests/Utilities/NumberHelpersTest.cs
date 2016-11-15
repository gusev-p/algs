using Algs.Utilities;
using NUnit.Framework;

namespace Algs.Tests.Utilities
{
    [TestFixture]
    public class NumberHelpersTest
    {
        [Test]
        public void Gcd()
        {
            Assert.That(NumberHelpers.Gcd(4, 6), Is.EqualTo(2));
            Assert.That(NumberHelpers.Gcd(17, 10), Is.EqualTo(1));
            Assert.That(NumberHelpers.Gcd(26, 65), Is.EqualTo(13));
        }
    }
}