using Compiler.Core.Parsing;

namespace Compiler.Frontend
{
    public class AstNode
    {
        public virtual AstNode Drain()
        {
            return this;
        }
        
        public Token Raw { get; set; }
    }
}