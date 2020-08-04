using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Core.Parsing;
using Compiler.Frontend.Ast;
using static Compiler.Core.Parsing.TokenType;

namespace Compiler.Frontend
{
    public static class Parser
    {
        private static List<(TokenType, Func<Token, AstNode>)> _typeConverterRules =
            new List<(TokenType, Func<Token, AstNode>)>
            {
                (TokenType.Number, (x) => new ExprNode {Value = new LiteralValueNode {Raw = x}}),
                (TokenType.Identifier, (x) => new ExprNode {Value = new IdentifierNode() {Raw = x}}),
            };


        private static List<List<(object[], Func<AstNode[], AstNode>)>> _passRules =
            new List<List<(object[], Func<AstNode[], AstNode>)>>
            {
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    (new object[] {typeof(ExprNode), Divide, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new DivisionNode {A = objs[0], B = objs[2]}}),

                    (new object[] {typeof(ExprNode), Multiply, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new MultiplicationNode {A = objs[0], B = objs[2]}}),
                },
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    (new object[] {typeof(ExprNode), Plus, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new AdditionNode {A = objs[0], B = objs[2]}}),

                    (new object[] {typeof(ExprNode), Minus, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new SubtractionNode {A = objs[0], B = objs[2]}}),
                }
            };

        public static DocumentNode Parse(TokenString ts)
        {
            var firstBuf = new List<AstNode>();

            //first we convert the toks into ast
            for (var i = 0; i < ts.Tokens.Count; i++)
            {
                firstBuf.Add(new AstNode {Raw = ts.Tokens[i]});
            }

            for (var i = 0; i < firstBuf.Count; i++)
            {
                var node = firstBuf[i];

                foreach (var (type, func) in _typeConverterRules)
                {
                    if (node.Raw.Type == type)
                    {
                        firstBuf[i] = func(node.Raw);
                    }
                }
            }


            var passCount = _passRules.Count;

            for (int p = 0; p < passCount; p++)
            {
                var passRules = _passRules[p];

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
                            if (!((o is Type t && firstBuf[i + (j - 1)].GetType() == t) ||
                                  (o is TokenType tt && firstBuf[i + (j - 1)].Raw?.Type == tt)
                                ))
                            {
                                flag = false;
                                break;
                            }
                            else
                            {
                                args.Add(firstBuf[i + (j - 1)]);
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


            return null; //nocommit
        }
    }
}