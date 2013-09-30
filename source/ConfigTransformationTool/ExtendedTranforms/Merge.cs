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
            var targetChildElement = targetElement.ChildNodes
                                                  .OfType<XmlElement>()
                                                  .FirstOrDefault(x => x.Name == transformElement.Name);
            if (targetChildElement == null)
            {
                targetElement.AppendChild(transformElement);
                return;
            }

            foreach (var transformChildElement in transformElement.ChildNodes.OfType<XmlElement>())
            {
                Apply(targetChildElement, transformChildElement);
            }
        }
    }
}