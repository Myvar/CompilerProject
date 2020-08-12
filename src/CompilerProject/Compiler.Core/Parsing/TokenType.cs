namespace Compiler.Core.Parsing
{
    public enum TokenType
    {
        // ReSharper disable once InconsistentNaming
        UNKNOWN,
        Error,

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
        CloseRoundBracket,

        OpenCurlyBracket,
        CloseCurlyBracket,
    }
}