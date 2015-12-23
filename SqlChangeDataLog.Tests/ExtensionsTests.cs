using NUnit.Framework;
using SqlChangeDataLog.Extensions;

namespace SqlChangeDataLog.Tests
{
    public class ExtensionsTests
    {
        [Test]
        public void StringApplyTemplateTest()
        {
            var tpl = "I'm travelling {one} the {two}";

            var o1 = new { one = "down", two = 12 };
            var o2 = new { one = 33, two = "wood" };

            Assert.AreEqual("I'm travelling down the 12", tpl.ApplyTemplate(o1));
            Assert.AreEqual("I'm travelling 33 the wood", tpl.ApplyTemplate(o2));
        }
    }
}
