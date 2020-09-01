namespace Compiler.Frontend.Ast
{
    public class ProcedureArgsNode : AstNode
    {
        public AstNode Args { get; set; }

        public override AstNode Drain()
        {
            return Args;
        }
    }
}