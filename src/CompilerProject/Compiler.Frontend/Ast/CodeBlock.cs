using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class CodeBlock : AstNode
    {
        public AstNode Body { get; set; }
    }

    public class CodeBlockList : AstNode
    {
        public List<AstNode> Body { get; set; } = new List<AstNode>();

        public AstNode A { get; set; }
        public AstNode B { get; set; }

        public override AstNode Drain()
        {
            Body.Clear();
            var x = A.Drain();
            var y = B.Drain();


            if (A is ListNode lna)
            {
                Body.AddRange(lna.Elements);
            }
            else
            {
                Body.Add(A);
            }

            if (B is ListNode lnb)
            {
                Body.AddRange(lnb.Elements);
            }
            else
            {
                Body.Add(B);
            }


            return this;
        }
    }
}