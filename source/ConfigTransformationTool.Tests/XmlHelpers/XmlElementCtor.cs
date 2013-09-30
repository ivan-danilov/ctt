using System;
using System.Collections.Generic;
using System.Text;

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    internal class XmlElementCtor : XmlNodeCtor
    {
        private readonly string _name;
        private int _cachedLevel = -1;
        private XmlElementCtor _parent;

        public XmlElementCtor(string name, params XmlNodeCtor[] content)
        {
            _name = name;
            IndentSize = 2;
            Children = new List<XmlElementCtor>();
            Attributes = new List<XmlAttributeCtor>();
            foreach (var nodeCtor in content)
                nodeCtor.AppendToElement(this);
        }

        public List<XmlElementCtor> Children { get; private set; }
        public List<XmlAttributeCtor> Attributes { get; private set; }

        public override void AppendToElement(XmlElementCtor element)
        {
            if (_parent != null) 
                throw new InvalidOperationException("This element was already appended elsewhere");
            _parent = element;
            element.Children.Add(this);
        }

        public override void Append(StringBuilder sb)
        {
            sb.Append(' ', GetLevel()*IndentSize);
            sb.AppendFormat("<{0}", _name);
            foreach (var attribute in Attributes)
            {
                sb.Append(' ');
                attribute.Append(sb);
            }
            if (Children.Count == 0)
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">\r\n");
                foreach (var child in Children)
                    child.Append(sb);
                sb.Append(' ', GetLevel() * IndentSize);
                sb.AppendFormat("</{0}>", _name);
            }

            // omit last end line before eof
            if (GetLevel() != 0)
                sb.Append("\r\n");
        }

        public XmlElementCtor WithIndent(int indentSize)
        {
            IndentSize = indentSize;
            return this;
        }

        public int IndentSize { get; set; }

        private int GetLevel()
        {
            if (_cachedLevel != -1) return _cachedLevel;
            if (_parent == null) return 0;
            return _cachedLevel = _parent.GetLevel() + 1;
        }
    }
}