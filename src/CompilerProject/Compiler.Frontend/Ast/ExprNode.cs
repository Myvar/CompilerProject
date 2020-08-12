using System.Diagnostics;

namespace Compiler.Frontend.Ast
{
  
    public class ExprNode : AstNode
    {
        public AstNode Value { get; set; }

        public override AstNode Drain()
        {
            return Value;
        }
    }
    
    
    public class StatementNode : AstNode
    {
        public AstNode Value { get; set; }

        public override AstNode Drain()
        {
            return Value;
        }
    }
}