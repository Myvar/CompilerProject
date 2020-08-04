using System.Collections.Generic;

namespace Compiler.Core.Parsing
{
    public class TokenString
    {
        public List<Token> Tokens { get; set; } = new List<Token>();
        public string Source { get; set; }
        public string SourceName { get; set; }
    }
}