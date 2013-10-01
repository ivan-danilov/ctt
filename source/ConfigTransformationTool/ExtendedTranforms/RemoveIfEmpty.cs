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
                if (element.Attributes.Count == 0 && element.ChildNodes.Cast<XmlNode>().All(n => n.NodeType == XmlNodeType.Whitespace))
                    TargetNode.RemoveChild(element);
            }
        }
    }
}