// ReSharper disable InconsistentNaming
//TODO Rename all enums

namespace Cslox
{
    internal enum SyntaxSign
    {
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_BRACE,
        RIGHT_BRACE,
        COMMA,
        DOT,
        MINUS,
        PLUS,
        SEMICOLON,
        SLASH,
        DOUBLE_QUOTE,
        STAR
    }

    internal enum LogicSign
    {
        BANG,
        BANG_EQUAL,
        EQUAL,
        EQUAL_EQUAL,
        GREATER,
        GREATER_EQUAL,
        LESS,
        LESS_EQUAL
    }

    internal enum LexicalSign
    {
        IDENTIFIER,
        STRING,
        NUMBER,
    }

    internal enum LingualSign
    {
        AND,
        CLASS,
        ELSE,
        FALSE,
        FUN,
        FOR,
        IF,
        NIL,
        OR,
        PRINT,
        RETURN,
        SUPER,
        THIS,
        TRUE,
        VAR,
        WHILE,
        EOF
    }

    public enum SymbolKind
    {
        SyntaxSign,
        LogicSign,
        LexicalSign
    }
}