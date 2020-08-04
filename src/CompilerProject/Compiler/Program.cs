using System;
using Compiler.Frontend;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = Lexer.Tokenize("./test.delta");

            /*foreach (var tok in ts.Tokens)
            {
                Console.WriteLine(
                    $"'{tok.Raw}':{tok.Type} [{tok.Line}] [{tok.Col}] [{tok.StartOffset}-{tok.EndOffset}]");
            }*/


            var ast = Parser.Parse(ts);
        }
    }
}