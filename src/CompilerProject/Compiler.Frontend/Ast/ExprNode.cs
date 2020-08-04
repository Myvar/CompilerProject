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

    public class LiteralValueNode : AstNode
    {
    }

    public class IdentifierNode : AstNode
    {
    }

    public class AdditionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }

    public class SubtractionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }

    public class DivisionNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }

    public class MultiplicationNode : AstNode
    {
        public AstNode A { get; set; }
        public AstNode B { get; set; }
    }
}