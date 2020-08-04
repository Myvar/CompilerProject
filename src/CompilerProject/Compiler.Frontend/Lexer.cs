using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Compiler.Core;
using Compiler.Core.Parsing;
using static Compiler.Core.Parsing.TokenType;

namespace Compiler.Frontend
{
    public static class Lexer
    {
        public static TokenString Tokenize(string fileName)
        {
            var fInfo = new FileInfo(fileName);
            return Tokenize(File.ReadAllText(fileName), fInfo.Name);
        }


        private static Dictionary<string, (TokenType, ConsoleColor)> _terminals =
            new Dictionary<string, (TokenType, ConsoleColor)>
            {
                {"=", (Eq, ConsoleColor.White)},
                {":=", (DoubleEq, ConsoleColor.White)},
                {"::", (DoubleDouble, ConsoleColor.White)},
                {":", (DoublePoint, ConsoleColor.White)},
                {"+", (Plus, ConsoleColor.White)},
                {"-", (Minus, ConsoleColor.White)},
                {"/", (Divide, ConsoleColor.White)},
                {"*", (Multiply, ConsoleColor.White)},
                {",", (Comma, ConsoleColor.White)},
                
                {"(", (OpenRoundBracket, ConsoleColor.White)},
                {")", (CloseRoundBracket, ConsoleColor.White)},
            };

        public static TokenString Tokenize(string raw, string fileName)
        {
            Logger.Log($"Started lexing '{fileName}'");

            var re = new TokenString();

            re.Tokens.Add(new Token()
            {
                Type = Sof
            });
            
            raw = "  " + raw + "  ";

            re.Source = raw;
            re.SourceName = fileName;

            var sw = new Stopwatch();
            sw.Start();

            var buf = new StringBuilder();


            var line = 1;
            var col = 0;

            int state = 0;

            for (var i = 1; i < raw.Length - 1; i++)
            {
                var b = raw[i - 1];
                var c = raw[i];
                var a = raw[i + 1];


                if (c == '\n')
                {
                    line++;
                    col = 0;
                }

                col++;
                switch (state)
                {
                    case 0:
                    {
                        foreach (var (key, (type, color)) in _terminals)
                        {
                            //is the remaining string long enuf to contain this key
                            if (raw.Length - i > key.Length)
                            {
                                var subStr = raw[(i)..(i + key.Length)];
                                if (key == subStr)
                                {
                                    re.Tokens.Add(new Token()
                                    {
                                        Col = col,
                                        Line = line,
                                        Color = color,
                                        Raw = subStr,
                                        Type = type,
                                        StartOffset = i,
                                        EndOffset = i + subStr.Length
                                    });

                                    i += key.Length - 1;
                                    col += key.Length - 1;
                                    goto done;
                                }
                            }
                        }

                        if (char.IsLetter(c))
                        {
                            state = 1;
                            buf.Append(c);
                        }

                        if (char.IsDigit(c))
                        {
                            state = 2;
                            buf.Append(c);
                        }


                        break;
                    }

                    case 1:
                    {
                        if (!char.IsLetterOrDigit(c) && c != '_')
                        {
                            state = 0;
                            re.Tokens.Add(new Token()
                            {
                                Col = col,
                                Line = line,
                                Color = ConsoleColor.White,
                                Raw = buf.ToString(),
                                Type = Identifier,
                                StartOffset = i - buf.Length,
                                EndOffset = i
                            });
                            i--;
                            buf.Clear();
                        }
                        else
                        {
                            buf.Append(c);
                        }

                        break;
                    }
                    case 2:
                    {
                        if (!char.IsDigit(c) && c != '_' && c != '.')
                        {
                            state = 0;
                            re.Tokens.Add(new Token()
                            {
                                Col = col,
                                Line = line,
                                Color = ConsoleColor.White,
                                Raw = buf.ToString(),
                                Type = Number,
                                StartOffset = i - buf.Length,
                                EndOffset = i
                            });
                            i--;
                            buf.Clear();
                        }
                        else
                        {
                            buf.Append(c);
                        }

                        break;
                    }
                }

                done: ;
            }

            sw.Stop();

            Logger.Debug($"Lexing took {sw.ElapsedMilliseconds}ms");

            re.Tokens.Add(new Token()
            {
                Type = Eof
            });
            
            return re;
        }
    }
}