namespace Compiler.Frontend.Ast
{
    public class IfNode : AstNode
    {
        public AstNode Condition { get; set; }
        public AstNode Body { get; set; }
    }
    
    public class LoopNode : AstNode
    {
        public AstNode Condition { get; set; }
        public AstNode Body { get; set; }
    }
}