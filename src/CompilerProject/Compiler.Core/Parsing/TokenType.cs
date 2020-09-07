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
        SemiColon,
        Arrow,
        Eq,
        At,
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