using System.Text;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    internal class XmlAttributeCtor : XmlNodeCtor
    {
        private readonly string _name;
        private readonly string _value;

        public XmlAttributeCtor(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public override void AppendToElement(XmlElementCtor element)
        {
            element.Attributes.Add(this);
        }

        public override void Append(StringBuilder sb)
        {
            sb.AppendFormat(@"{0}=""{1}""", _name, _value);
        }
    }
}