namespace Compiler.Frontend.Ast
{
    public class NameTypePair : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode Type { get; set; }
    }
}