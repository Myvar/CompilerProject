namespace Compiler.Core.Parsing
{
    public enum TokenType
    {
        // ReSharper disable once InconsistentNaming
        UNKNOWN,
        Sof,
        Eof,

        Identifier,
        Number,

        DoubleEq,
        DoubleDouble,
        DoublePoint,
        Eq,
        Plus,
        Minus,
        Divide,
        Multiply,
        Comma,

        OpenRoundBracket,
        CloseRoundBracket
    }
}