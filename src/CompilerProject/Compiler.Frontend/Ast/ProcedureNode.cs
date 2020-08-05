namespace Compiler.Frontend.Ast
{
    public class ProcedureArgsNode : AstNode
    {
        public AstNode Args { get; set; }

        /*public override AstNode Drain()
        {
            return Args;
        }*/
    }
    
    public class ProcedureNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Args { get; set; }
        public AstNode Body { get; set; }

        public override AstNode Drain()
        {
            if (Name is ProcedureNode node)
            {
                Name = node.Name;
            }
            return this;
        }
    }
}