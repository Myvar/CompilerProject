namespace Compiler.Core.Parsing
{
    public enum TokenType
    {
        // ReSharper disable once InconsistentNaming
        UNKNOWN,
        Error,
        GreaterThan,
        LessThan,
        Sof,
        Eof,
        EqBool,
        Identifier,
        Number,
        If,
        Loop,
        DoubleEq,
        DoubleDouble,
        DoublePoint,
        AndBool,
        OrBool,
        AndBitWise,
        Not,
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