namespace Compiler.Frontend.Ast
{
    public class ComparisonNode : AstNode
    {
        public AstNode Operator { get; set; }
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}