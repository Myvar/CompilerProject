namespace Compiler.Frontend.Ast
{
    public class UnaryOperator : AstNode
    {
        public AstNode Operator { get; set; }
        public AstNode Argument { get; set; }
    }
}