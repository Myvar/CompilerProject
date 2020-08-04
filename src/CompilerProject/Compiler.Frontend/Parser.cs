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
                (Number, (x) => new ExprNode {Value = new LiteralValueNode {Raw = x}}),
                (Identifier, (x) => new ExprNode {Value = new IdentifierNode() {Raw = x}}),
            };


        private static List<List<(object[], Func<AstNode[], AstNode>)>> _passRules =
            new List<List<(object[], Func<AstNode[], AstNode>)>>
            {
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    //
                    // NameTypePair
                    //
                    (new object[] {Identifier, DoublePoint, Identifier},
                        (objs) => new NameTypePair() {Name = objs[0], Type = objs[2]}),
                    
                    //
                    // Const Name ProcedureNode
                    //
                    (new object[] {Identifier, DoubleDouble},
                        (objs) => new ProcedureNode() {Name = objs[0]}),
                },
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    //
                    // DivisionNode
                    //
                    (new object[] {typeof(ExprNode), Divide, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new DivisionNode {A = objs[0], B = objs[2]}}),

                    //
                    // MultiplicationNode
                    //
                    (new object[] {typeof(ExprNode), Multiply, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new MultiplicationNode {A = objs[0], B = objs[2]}}),
                },
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    //
                    // AdditionNode
                    //
                    (new object[] {typeof(ExprNode), Plus, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new AdditionNode {A = objs[0], B = objs[2]}}),

                    //
                    // SubtractionNode
                    //
                    (new object[] {typeof(ExprNode), Minus, typeof(ExprNode)},
                        (objs) => new ExprNode
                            {Value = new SubtractionNode {A = objs[0], B = objs[2]}}),

                    //
                    // ListNode A
                    //
                    (new object[] {typeof(NameTypePair), Comma, typeof(NameTypePair)},
                        (objs) => new ListNode() {A = objs[0], B = objs[2]}),
                },
                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    //
                    // AssignmentNode
                    //
                    (new object[] {typeof(ExprNode), Eq, typeof(ExprNode)},
                        (objs) => new AssignmentNode() {Name = objs[0], Value = objs[2]}),

                    //
                    // DeclNode
                    //
                    (new object[] {typeof(ExprNode), DoubleEq, typeof(ExprNode)},
                        (objs) => new DeclNode() {Name = objs[0], Value = objs[2]}),

                    //
                    // ListNode B
                    //
                    (new object[] {typeof(ListNode), Comma, typeof(NameTypePair)},
                        (objs) => new ListNode() {A = objs[0], B = objs[2]}),

                    //
                    // ProcedureArgsNode List
                    //
                    (new object[] {OpenRoundBracket, typeof(ListNode), CloseRoundBracket},
                        (objs) => new ProcedureArgsNode() {Args = objs[1]}),
                    
                    //
                    // ProcedureArgsNode Pair
                    //
                    (new object[] {OpenRoundBracket, typeof(NameTypePair), CloseRoundBracket},
                        (objs) => new ProcedureArgsNode() {Args = objs[1]}),
                },

                new List<(object[], Func<AstNode[], AstNode>)>
                {
                    //
                    // ProcedureNode
                    //
                    (new object[] {typeof(ProcedureNode), typeof(ProcedureArgsNode)},
                        (objs) => new ProcedureNode() {Name = objs[0], Args = objs[1]}),
                    
                },
            };

        public static DocumentNode Parse(TokenString ts)
        {
            var firstBuf = new List<AstNode>();

            //first we convert the toks into ast
            for (var i = 0; i < ts.Tokens.Count; i++)
            {
                firstBuf.Add(new AstNode {Raw = ts.Tokens[i]});
            }


            var passCount = _passRules.Count;

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