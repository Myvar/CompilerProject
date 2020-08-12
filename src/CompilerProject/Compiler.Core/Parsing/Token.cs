using System;

namespace Compiler.Core.Parsing
{
    public class Token
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }

        public int Line { get; set; }
        public int Col { get; set; }

        public TokenType Type { get; set; }
        public ConsoleColor Color { get; set; }

        public string Raw { get; set; }

        public TokenString Owner { get; set; }
    }
}