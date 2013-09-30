using System.Text;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    internal abstract class XmlNodeCtor
    {
        public abstract void AppendToElement(XmlElementCtor element);

        public abstract void Append(StringBuilder sb);
    }
}