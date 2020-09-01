namespace Compiler.Frontend.Ast
{
    public class AttributeNodeName : AstNode
    {
        public AstNode Name { get; set; }
    }

    public class AttributeNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Client { get; set; }

        public override AstNode Drain()
        {
            if (Name is AttributeNodeName ann)
            {
                Client.Attributes.Add(ann.Name.Raw.Raw);
            }

            return Client;
            
        }
    }
}