using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Core.Parsing;
using Compiler.Frontend.Ast;
using Compiler.Frontend.Grammer;
using static Compiler.Core.Parsing.TokenType;

namespace Compiler.Frontend
{
    public static class Parser
    {
        private static List<(TokenType, Func<Token, AstNode>)> _typeConverterRules =
            new List<(TokenType, Func<Token, AstNode>)>
            {
                (Number, (x) => new ExprNode {Value = new LiteralValueNode {Raw = x}}),
                (Identifier, (x) => new ExprNode {Value = new IdentifierNode() {Raw = x}}),
            };


     

        public static DocumentNode Parse(TokenString ts)
        {
            var firstBuf = new List<AstNode>();

            //first we convert the toks into ast
            for (var i = 0; i < ts.Tokens.Count; i++)
            {
                firstBuf.Add(new AstNode {Raw = ts.Tokens[i]});
            }


            var passCount = Rules.PassRules.Count;

            for (int p = 0; p < passCount; p++)
            {
                if (p == 1)
                {
                    for (var i = 0; i < firstBuf.Count; i++)
                    {
                        var node = firstBuf[i];

                        foreach (var (type, func) in _typeConverterRules)
                        {
                            if (node.Raw?.Type == type)
                            {
                                firstBuf[i] = func(node.Raw);
                            }
                        }
                    }
                }

                var passRules = Rules.PassRules[p];

                start_over:
                for (var i = 1; i < firstBuf.Count - 1; i++)
                {
                    if (firstBuf[i] == null) continue;


                    foreach (var (objects, func) in passRules)
                    {
                        var flag = true;

                        var args = new List<AstNode>();

                        for (var j = 0; j < objects.Length; j++)
                        {
                            var o = objects[j];
                            if ((o is Type t && firstBuf[i + (j - 1)].GetType().Name == t.Name) ||
                                (o is TokenType tt && firstBuf[i + (j - 1)].Raw?.Type == tt)
                            )
                            {
                                args.Add(firstBuf[i + (j - 1)]);
                            }
                            else
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            var ag = args.ToArray().ToList();

                            for (var index = 0; index < args.Count; index++)
                            {
                                args[index] = args[index].Drain();
                            }

                            firstBuf.Insert(i, func(args.ToArray()));

                            foreach (var i1 in ag)
                            {
                                firstBuf.Remove(i1);
                            }


                            goto start_over;
                        }
                    }
                }
            }

            var re = new DocumentNode();

            foreach (var node in firstBuf)
            {
                if (node.Raw?.Type == Sof || node.Raw?.Type == Eof) continue;

                re.Statments.Add(node.Drain());
            }


            return re;
        }
    }
}