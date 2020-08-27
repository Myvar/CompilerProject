using System;
using System.Linq;
using Compiler.Frontend;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = Lexer.Tokenize("./test.delta");


            var ast = Parser.Parse(ts);


            /*foreach (var tok in ts.Tokens)
            {
                Console.WriteLine(
                    $"'{tok.Raw}':{tok.Type} [{tok.Line}] [{tok.Col}] [{tok.StartOffset}-{tok.EndOffset}]");
            }*/
             ast.DebugPrint();

            if (Report.Reports.Any())
            {
                Console.WriteLine("-------------------------------------------------------------------------------------------");
                Report.PrintAll();
            }
        }
    }
}