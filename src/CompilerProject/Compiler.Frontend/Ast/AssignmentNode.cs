namespace Compiler.Frontend.Ast
{
    public class AssignmentNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Value { get; set; }
    }
}