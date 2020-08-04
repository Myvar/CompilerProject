namespace Compiler.Frontend.Ast
{
    public class AdditionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}