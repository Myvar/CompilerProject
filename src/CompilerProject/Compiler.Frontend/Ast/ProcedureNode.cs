using Compiler.Core;

namespace Compiler.Frontend.Ast
{
    public class ProcedureNode : AstNode
    {
        public AstNode Name { get; set; }
        public AstNode ReturnType { get; set; }
        public AstNode Args { get; set; }
        public AstNode Body { get; set; }
     

        public override AstNode Drain()
        {
            if (Name is ProcedureNode node)
            {
                Name = node.Name;

                Scope.Procedure.Add(node.Name.Raw.Raw);
            }

            return this;
        }
    }
}