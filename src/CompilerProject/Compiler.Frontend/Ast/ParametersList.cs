using System.Collections.Generic;

namespace Compiler.Frontend.Ast
{
    public class ParametersList : AstNode
    {
        public List<AstNode> Elements { get; set; } = new List<AstNode>();

        public AstNode A { get; set; }
        public AstNode B { get; set; }

        public override AstNode Drain()
        {
            Elements.Clear();
            var x = A.Drain();
            var y = B?.Drain();


            if (A is ParametersList lna)
            {
                Elements.AddRange(lna.Elements);
            }
            else
            {
                Elements.Add(A);
            }

            if (B is ParametersList lnb)
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