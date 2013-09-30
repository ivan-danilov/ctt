using System;
using System.Text;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    internal class XmlDocCtor
    {
        private readonly string _xmlDeclaration;
        private readonly XmlElementCtor _rootCtor;

        public XmlDocCtor(XmlElementCtor rootCtor)
            : this(@"<?xml version=""1.0"" encoding=""utf-8""?>", rootCtor)
        {
        }

        public XmlDocCtor(string xmlDeclaration, XmlElementCtor rootCtor)
        {
            _xmlDeclaration = xmlDeclaration;
            _rootCtor = rootCtor;
        }

        public string MakeString()
        {
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(_xmlDeclaration))
                sb.AppendFormat("{0}\r\n", _xmlDeclaration);
            _rootCtor.Append(sb);
            return sb.ToString();
        }
    }
}