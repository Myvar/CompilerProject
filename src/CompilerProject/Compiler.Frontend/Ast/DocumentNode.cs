using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class DocumentNode
    {
        public List<AstNode> Statments { get; set; } = new List<AstNode>();
    }
}