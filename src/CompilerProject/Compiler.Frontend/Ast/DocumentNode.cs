using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class DocumentNode : AstNode
    {
        public List<AstNode> Statments { get; set; } = new List<AstNode>();
    }
}