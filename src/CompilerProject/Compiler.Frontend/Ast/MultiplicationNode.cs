namespace Compiler.Frontend.Ast
{
    public class MultiplicationNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}