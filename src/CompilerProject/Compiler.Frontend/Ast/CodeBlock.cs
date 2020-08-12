using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class CodeBlock : AstNode
    {
        public AstNode Body { get; set; }
        public override AstNode Drain()
        {
            return Body;
        }
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


            if (A is CodeBlockList lna)
            {
                Body.AddRange(lna.Body);
            }
            else
            {
                Body.Add(A);
            }

            if (B is CodeBlockList lnb)
            {
                Body.AddRange(lnb.Body);
            }
            else
            {
                Body.Add(B);
            }


            return this;
        }
    }
}