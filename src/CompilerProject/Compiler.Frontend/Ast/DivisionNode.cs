namespace Compiler.Frontend.Ast
{
    public class DivisionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}