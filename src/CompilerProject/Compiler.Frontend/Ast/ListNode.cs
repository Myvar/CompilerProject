using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class ListNode : AstNode
    {
        public List<AstNode> Elements { get; set; } = new List<AstNode>();

        public AstNode A { get; set; }
        public AstNode B { get; set; }

        public override AstNode Drain()
        {
            Elements.Clear();
            var x = A.Drain();
            var y = B.Drain();

            
            if (A is ListNode lna)
            {
                Elements.AddRange(lna.Elements);
            }
            else
            {
                Elements.Add(A);
            }
            
            if (B is ListNode lnb)
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