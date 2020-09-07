namespace Compiler.Frontend.Ast
{
    public class OrBoolNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}