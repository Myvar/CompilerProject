using System;
using System.Linq;
using Compiler.Core;
using Compiler.Frontend;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = Lexer.Tokenize("./test.delta");


            var ast = Parser.Parse(ts);


            foreach (var tok in ts.Tokens)
            {
                Console.WriteLine(
                    $"'{tok.Raw}':{tok.Type} [{tok.Line}] [{tok.Col}] [{tok.StartOffset}-{tok.EndOffset}]");
            }

            ast.DebugPrint();

            foreach (var procedure in Scope.Procedure)
            {
                Console.WriteLine($"Found Procedure: {procedure}");
            }


            if (Report.Reports.Any())
            {
                Console.WriteLine(
                    "-------------------------------------------------------------------------------------------");
                Report.PrintAll();
            }
        }
    }
}