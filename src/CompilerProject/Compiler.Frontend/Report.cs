using System;
using System.Collections.Generic;
using System.Linq;
using Compiler.Core.Parsing;

namespace Compiler.Frontend
{
    public class Report
    {
        public static List<Report> Reports { get; set; } = new List<Report>();

        private static Stack<List<Report>> _reportStack = new Stack<List<Report>>();

        public static void Push()
        {
            _reportStack.Push(Reports.ToArray().ToList());
        }

        public static void Pop()
        {
            Reports = _reportStack.Pop();
        }

        public static void PrintAll()
        {
            foreach (var report in Reports)
            {
                report.Print();
            }
        }

        public ReportType Type { get; set; }
        public string DisplayMessage { get; set; }
        public Token Token { get; set; }
        public List<string> Suggestions { get; set; } = new List<string>();

        public static Report Error(Token token)
        {
            var re = new Report();
            re.Type = ReportType.Error;
            re.Token = token;
            Reports.Add(re);
            return re;
        }

        public static Report Warning(Token token)
        {
            var re = new Report();
            re.Type = ReportType.Warning;
            re.Token = token;
            Reports.Add(re);
            return re;
        }

        public static Report Message(Token token)
        {
            var re = new Report();
            re.Type = ReportType.Message;
            re.Token = token;
            Reports.Add(re);
            return re;
        }

        public Report Message(string msg)
        {
            DisplayMessage = msg;
            return this;
        }

        public Report Suggestion(string msg)
        {
            Suggestions.Add(msg);
            return this;
        }

        public void Print()
        {
            var width = Console.BufferWidth;
            if (width > 150)
            {
                width = width / 2;
            }

            Console.Write("Error Message: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\"{DisplayMessage}\" ");
            Console.ResetColor();
            Console.Write($"[TokenType: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Token.Type}");
            Console.ResetColor();
            Console.Write($"] [Value:");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"\"{Token.Raw}\"");
            Console.ResetColor();
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{Token.Owner.SourceName}│L{(Token.Line + 1)}:C{Token.Col}");
            Console.ResetColor();
            Console.WriteLine("".PadLeft(width, '-'));
            var lines = 5;


            var x = Token.Owner.Source.Split('\n');

            var offset = 1;
            for (var l = 0; l < x.Length; l++)
            {
                if (Token.Line - 3 <= l  && (Token.Line + lines - 2) >= l )
                {
                    Console.Write((l + 1) + "│ ");
                }

                var s = x[l];
                for (var i = 0; i < s.Length; i++)
                {
                    var c = s[i];
                    if (Token.Line - 3 <= l  && (Token.Line + lines - 2) >= l )
                    {
                        foreach (var t in Token.Owner.Tokens)
                        {
                            if (offset >= Token.StartOffset &&
                                offset <= Token.EndOffset)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;
                            }

                            if (offset >= t.StartOffset &&
                                offset <= t.EndOffset)
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                switch (t.Type)
                                {
                                    case TokenType.Error:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        break;
                                        /*case TokenType.Comment:
                                            Console.ForegroundColor = ConsoleColor.DarkGreen;*/
                                        break;
                                        /*case TokenType.String:
                                            Console.ForegroundColor = ConsoleColor.DarkRed;*/
                                        break;
                                    case TokenType.Number:
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        break;
                                    case TokenType.Identifier:
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        break;
                                    /*case TokenType.HashTag:
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        break;*/
                                    // case TokenType.DotDot:
                                    case TokenType.DoublePoint:
                                        /*case TokenType.DoublePointEq:
                                        case TokenType.DoubleDoublePoint:
                                        case TokenType.Arrow:*/
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        break;
                                    /*case TokenType.If:
                                    case TokenType.Loop:
                                    case TokenType.Else:
                                    case TokenType.Struct:
                                        Console.ForegroundColor = ConsoleColor.Cyan;
                                        break;*/
                                }

                                break;
                            }
                        }

                        //Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write(c);
                        Console.ResetColor();
                    }

                    offset++;
                }

                offset++;
                if (Token.Line - 3 <= l && (Token.Line + lines - 2) >= l) Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine();
            if (Suggestions.Count > 0)
            {
                Console.WriteLine("".PadLeft(width, '-'));
                Console.WriteLine("   Suggestions:");
                Console.WriteLine("".PadLeft(width, '-'));
                for (var i = 0; i < Suggestions.Count; i++)
                {
                    var suggestion = Suggestions[i];
                    Console.WriteLine($"{(i + 1).ToString().PadLeft(3)}: " + suggestion);
                }
            }

            Console.WriteLine("".PadLeft(width, '-'));
        }
    }
}