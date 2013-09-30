using System;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace OutcoldSolutions.ConfigTransformationTool.ExtendedTranforms
{
    public class AttributeRegexReplace : Transform
    {
        private string _pattern;
        private string _replacement;
        private string _attributeName;

        protected string AttributeName
        {
            get { return _attributeName ?? (_attributeName = GetArgumentValue("Attribute")); }
        }

        protected string Pattern
        {
            get { return _pattern ?? (_pattern = GetArgumentValue("Pattern")); }
        }

        protected string Replacement
        {
            get { return _replacement ?? (_replacement = GetArgumentValue("Replacement")); }
        }

        protected string GetArgumentValue(string name)
        {
            // this extracts a value from the arguments provided
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            string result = null;
            if (Arguments != null && Arguments.Count > 0)
            {
                foreach (string arg in Arguments)
                {
                    if (!string.IsNullOrWhiteSpace(arg))
                    {
                        string trimmedArg = arg.Trim();
                        if (trimmedArg.ToUpperInvariant().StartsWith(name.ToUpperInvariant()))
                        {
                            int start = arg.IndexOf('\'');
                            int last = arg.LastIndexOf('\'');
                            if (start <= 0 || last <= 0 || last <= 0)
                            {
                                throw new ArgumentException("Expected two ['] characters");
                            }

                            string value = trimmedArg.Substring(start, last - start);
                            // remove any leading or trailing '
                            result = value.Trim().TrimStart('\'').TrimStart('\'');
                        }
                    }
                }
            }
            return result;
        }

        protected override void Apply()
        {
            foreach (XmlAttribute att in TargetNode.Attributes)
            {
                if (string.Compare(att.Name, AttributeName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    // get current value, perform the Regex
                    att.Value = Regex.Replace(att.Value, Pattern, Replacement);
                }
            }
        }
    }
}