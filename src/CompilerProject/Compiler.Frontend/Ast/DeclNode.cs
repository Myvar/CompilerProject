namespace Compiler.Frontend.Ast
{
    public class DeclNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Value { get; set; }
    }
}