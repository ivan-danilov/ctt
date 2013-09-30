using NUnit.Framework;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    [TestFixture]
    public class XmlHelpersTests
    {
        [Test]
        public void BasicElementWithDefaultDeclaration()
        {
            var str = El("A").MakeString();
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<A />", str);
        }

        [Test]
        public void ElementWithAttributes()
        {
            var str = El("A", At("a", "v1"), At("b", "v2")).MakeString(null);
            Assert.AreEqual("<A a=\"v1\" b=\"v2\" />", str);
        }

        [Test]
        public void ElementWithChildrenElements()
        {
            var str = El("A", El("M", El("X")), El("N")).MakeString(null);
            var expected = "<A>\r\n  <M>\r\n    <X />\r\n  </M>\r\n  <N />\r\n</A>";
            Assert.AreEqual(expected, str);
        }

        private XmlElementCtor El(string name, params XmlNodeCtor[] content)
        {
            return new XmlElementCtor(name, content);
        }

        private XmlAttributeCtor At(string name, string value)
        {
            return new XmlAttributeCtor(name, value);
        }
    }
}