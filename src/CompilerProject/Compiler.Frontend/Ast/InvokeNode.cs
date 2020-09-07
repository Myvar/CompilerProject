using System.Collections.Generic;

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

    public class InvokeNodeName : AstNode
    {
        public AstNode Target { get; set; }

        public override AstNode Drain()
        {
            return Target;
        }
    }
    
    public class InvokeListNode : AstNode
    {
        public List<AstNode> Elements { get; set; } = new List<AstNode>();

        public AstNode A { get; set; }
        public AstNode B { get; set; }

        public override AstNode Drain()
        {
            Elements.Clear();
            var x = A.Drain();
            var y = B.Drain();


            if (A is InvokeListNode lna)
            {
                Elements.AddRange(lna.Elements);
            }
            else
            {
                Elements.Add(A);
            }

            if (B is InvokeListNode lnb)
            {
                Elements.AddRange(lnb.Elements);
            }
            else
            {
                Elements.Add(B);
            }


            return this;
        }
    }
}