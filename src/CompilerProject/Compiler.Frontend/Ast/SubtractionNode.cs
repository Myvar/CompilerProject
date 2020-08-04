namespace Compiler.Frontend.Ast
{
    public class SubtractionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}