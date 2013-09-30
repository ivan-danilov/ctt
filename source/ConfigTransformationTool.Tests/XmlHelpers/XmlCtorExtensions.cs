namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    internal static class XmlCtorExtensions
    {
        public static string MakeString(this XmlElementCtor root, string xmlDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>")
        {
            return new XmlDocCtor(xmlDeclaration, root).MakeString();
        }
    }
}