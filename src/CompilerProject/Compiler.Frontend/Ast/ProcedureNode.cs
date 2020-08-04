namespace Compiler.Frontend.Ast
{
    public class ProcedureNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Args { get; set; }
    }
}