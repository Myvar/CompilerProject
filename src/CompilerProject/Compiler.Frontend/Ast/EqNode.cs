namespace Compiler.Frontend.Ast
{
    public class EqNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}