namespace Compiler.Frontend.Ast
{
    public class CodeBlock : AstNode
    {
        public AstNode Body { get; set; }
        public override AstNode Drain()
        {
            return Body;
        }
    }
}