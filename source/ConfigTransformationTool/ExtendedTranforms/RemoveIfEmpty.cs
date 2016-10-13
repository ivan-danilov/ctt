using System.Linq;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace OutcoldSolutions.ConfigTransformationTool.ExtendedTranforms
{
    public class RemoveIfEmpty : Transform
    {
        public RemoveIfEmpty()
            :base(TransformFlags.UseParentAsTargetNode, MissingTargetMessage.Error)
        {
        }

        protected override void Apply()
        {
            if (TargetChildNodes == null) return;
            foreach (var element in TargetChildNodes.OfType<XmlElement>())
            {
                if (element.ChildNodes.Cast<XmlNode>()
                           .Any(n => n.NodeType != XmlNodeType.Whitespace &&
                                     n.NodeType != XmlNodeType.Comment &&
                                     n.NodeType != XmlNodeType.Attribute))
                {
                    continue;
                }
                TargetNode.RemoveChild(element);
            }
        }
    }
}