namespace Compiler.Frontend.Ast
{
    public class AndBitWiseNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
    
    public class AndBoolNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}