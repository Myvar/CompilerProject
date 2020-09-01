namespace Compiler.Frontend.Ast
{
    public class InvokeNode : AstNode
    {
        public AstNode Target { get; set; }
        public AstNode Arguments { get; set; }

        /*public override AstNode Drain()
        {
            if (Target is InvokeNode inn)
            {
                Target = inn.Target;
            }

            return base.Drain();
        }*/
    }
}