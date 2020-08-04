using System.Diagnostics;

namespace Compiler.Frontend.Ast
{
    [DebuggerDisplay("{Value.GetType().Name}")]
    public class ExprNode : AstNode
    {
        public AstNode Value { get; set; }

        public override AstNode Drain()
        {
            return Value;
        }
    }
}