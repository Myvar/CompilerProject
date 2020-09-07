using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Core;
using Compiler.Core.Parsing;
using Compiler.Frontend.Ast;
using Compiler.Frontend.Grammer;
using DotNetGraph;
using DotNetGraph.Extensions;
using DotNetGraph.Node;
using DotNetGraph.SubGraph;
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
            var sw = new Stopwatch();
            sw.Start();
            var firstBuf = new List<AstNode>();

            //first we convert the toks into ast
            for (var i = 0; i < ts.Tokens.Count; i++)
            {
                firstBuf.Add(new AstNode {Raw = ts.Tokens[i], ReportToken = ts.Tokens[i]});
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
                                firstBuf[i].ReportToken = node.ReportToken;
                                firstBuf[i].Expected = "";
                            }
                        }
                    }
                }

                //nocommit
                var passRules = Rules.PassRules[p];

                start_over:
                for (var i = 1; i < firstBuf.Count - 1; i++)
                {
                    if (firstBuf[i] == null) continue;

                    var startOver = false;
                    // Parallel.ForEach(passRules, tuple =>
                    foreach (var (objects, func) in passRules)
                    {
                        // var (objects, func) = tuple;
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
                                if (j >= firstBuf[i + (j - 1)].ClosestExpected)
                                {
                                    firstBuf[i + (j - 1)].ClosestExpected = j;
                                    firstBuf[i + (j - 1)].Expected = o switch
                                    {
                                        Type type => type.Name,
                                        TokenType toketype => toketype.ToString(),
                                        _ => "Unknown"
                                    };
                                    if (firstBuf[i + (j - 1)].Raw != null)
                                    {
                                        firstBuf[i + (j - 1)].Found = firstBuf[i + (j - 1)].Raw;
                                    }
                                    else
                                    {
                                        firstBuf[i + (j - 1)].Found = firstBuf[i + (j - 1)].ReportToken;
                                    }
                                }

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
                                if (args[index] != null) args[index].Parsed = true;
                            }


                            var res = func(args.ToArray());
                            res.Parsed = true;

                            res.ReportToken = ag[0].ReportToken;

                            firstBuf.Insert(i, res);


                            foreach (var i1 in ag)
                            {
                                firstBuf.Remove(i1);
                            }

                            startOver = true;

                            goto exit;
                        }

                        exit: ;
                    }
                    //);

                    if (startOver)
                    {
                        startOver = false;
                        goto start_over;
                    }
                }
            }

            var re = new DocumentNode();

            foreach (var node in firstBuf)
            {
                if (node.Raw?.Type == Sof || node.Raw?.Type == Eof) continue;

                re.Statments.Add(node.Drain());
            }

            //now find errors

            //if there are more than 1 million steps/errors in this syntax then well just give up wtf
            var lowestEidx = 1000000;
            AstNode ast = null;

            foreach (var node in firstBuf)
            {
                if (node.ReportToken?.Type == Eof || node.ReportToken?.Type == Sof) continue;


                if (!node.Parsed && node.ReportToken != null && node.Expected != "")
                {
                    if (node.ClosestExpected < lowestEidx)
                    {
                        lowestEidx = node.ClosestExpected;
                        ast = node;
                    }

                    Logger.Debug(
                        $"Found Un-parsed Node '{node.ReportToken.Raw}' at [L{node.ReportToken.Line}C{node.ReportToken.Col}] of {node.ReportToken.Type} expected {node?.Expected} eidx [{node.ClosestExpected}] found {node?.Found?.Type}");
                }
            }

            if (ast != null)
            {
                Logger.Error(
                    $"Found Un-parsed Node '{ast.ReportToken.Raw}' at [L{ast.ReportToken.Line}C{ast.ReportToken.Col}] of {ast.ReportToken.Type} expected {ast?.Expected} found {ast?.Found?.Type}");

                //    ast.ReportToken.Type = Error;

                Report
                    .Error(ast.ReportToken)
                    .Message(
                        $"Found Un-parsed Node '{ast.ReportToken.Raw}' at [L{ast.ReportToken.Line}C{ast.ReportToken.Col}] of {ast.ReportToken.Type} expected {ast?.Expected} found {ast?.Found?.Type}")
                    .Suggestion(
                        "You should have listened to your mom and studied law but no you want to be a programmer...")
                    .Suggestion("Look on stackover flow")
                    .Suggestion("Call that one friend who actually knows what he is doing")
                    .Suggestion(
                        "Remember the 5 stages of debugging:  1. Denial 2. Bargaining 3. Anger 4. Deperstion 5. Acceptance.")
                    .Suggestion("Plant a garden, get a pet, go outside for once")
                    .Suggestion("Call it a feature and go play some games")
                    ;
            }

            sw.Stop();

            Logger.Log($"Parsing took {sw.ElapsedMilliseconds}ms");

            return re;
        }
    }
}