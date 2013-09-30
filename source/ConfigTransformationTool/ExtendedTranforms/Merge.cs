using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace OutcoldSolutions.ConfigTransformationTool.ExtendedTranforms
{
    public class Merge : Transform
    {
        public Merge()
            : base(TransformFlags.UseParentAsTargetNode)
        {
        }

        protected override void Apply()
        {
            Apply((XmlElement) TargetNode, (XmlElement) TransformNode);
        }

        public void Apply(XmlElement targetElement, XmlElement transformElement)
        {
            var args = Arguments ?? Enumerable.Empty<string>();

            var idx = FindIndex(args, arg => !transformElement.HasAttribute(arg));
            if (idx != -1)
                throw new ArgumentException(String.Format("Attribute {0} is not found", Arguments[idx]));

            var targetChildElement = targetElement.ChildNodes
                                                  .OfType<XmlElement>()
                                                  .FirstOrDefault(x => x.Name == transformElement.Name &&
                                                                       args.All(arg => transformElement.GetAttribute(arg) == x.GetAttribute(arg)));
            if (targetChildElement == null)
            {
                targetElement.AppendChild(transformElement);
            }
            else
            {
                foreach (XmlAttribute attr in transformElement.Attributes)
                {
                    targetChildElement.SetAttribute(attr.Name, attr.Value);
                }
            }
        }

        private static int FindIndex<T>(IEnumerable<T> source, Predicate<T> criteria)
        {
            int current = 0;
            foreach (var element in source)
            {
                if (criteria(element))
                    return current;
                current++;
            }
            return -1;
        }
    }
}