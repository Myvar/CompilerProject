namespace Compiler.Frontend.Ast
{
    public class StatementNode : AstNode
    {
        public AstNode Value { get; set; }

        public override AstNode Drain()
        {
            return Value;
        }
    }
}