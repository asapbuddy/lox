using System.Collections.Generic;

namespace Cslox
{
    internal class LookupTable
    {
        private readonly Dictionary<string, LingualSign> _lingual = new Dictionary<string, LingualSign>
        {
            {"and", LingualSign.AND},
            {"class", LingualSign.CLASS},
            {"else", LingualSign.ELSE},
            {"false", LingualSign.FALSE},
            {"for", LingualSign.FOR},
            {"fun", LingualSign.FUN},
            {"if", LingualSign.IF},
            {"nil", LingualSign.NIL},
            {"or", LingualSign.OR},
            {"print", LingualSign.PRINT},
            {"return", LingualSign.RETURN},
            {"super", LingualSign.SUPER},
            {"this", LingualSign.THIS},
            {"true", LingualSign.TRUE},
            {"var", LingualSign.VAR},
            {"while", LingualSign.WHILE},
        };

        private readonly Dictionary<char, SyntaxSign> _syntax = new Dictionary<char, SyntaxSign>
        {
            {'(', SyntaxSign.LEFT_PAREN},
            {')', SyntaxSign.RIGHT_PAREN},
            {'{', SyntaxSign.LEFT_BRACE},
            {'}', SyntaxSign.RIGHT_BRACE},
            {',', SyntaxSign.COMMA},
            {'.', SyntaxSign.DOT},
            {'-', SyntaxSign.MINUS},
            {'+', SyntaxSign.PLUS},
            {';', SyntaxSign.SEMICOLON},
            {'*', SyntaxSign.STAR},
            {'/', SyntaxSign.SLASH},
            {'"', SyntaxSign.DOUBLE_QUOTE}
        };

        private readonly Dictionary<char, LogicSign> _logic = new Dictionary<char, LogicSign>
        {
            {'!', LogicSign.BANG},
            {'=', LogicSign.EQUAL},
            {'>', LogicSign.GREATER},
            {'<', LogicSign.LESS}
        };

        public SymbolKind GetSymbolKind(char ch)
        {
            if (_logic.ContainsKey(ch))
                return SymbolKind.LogicSign;
            if (_syntax.ContainsKey(ch))
                return SymbolKind.SyntaxSign;
            return SymbolKind.LexicalSign;
        }

        public SyntaxSign GetSyntaxToken(char ch)
        {
            return _syntax[ch];
        }

        public LogicSign GetLogicToken(char ch)
        {
            return _logic[ch];
        }

        private bool IsAlpha(char ch)
        {
            return ch >= 'a' && ch <= 'z' ||
                   ch >= 'A' && ch <= 'Z' ||
                   ch == '_';
        }

        private bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private bool IsAlphaNumeric(char ch)
        {
            return IsAlpha(ch) || IsDigit(ch);
        }


        private bool IsDelimiter(char ch)
        {
            return ch == ' ' || ch == '\r' || ch == '\n';
        }

        private bool IsSlash(char ch)
        {
            return ch == '/';
        }

        private bool IsString(char ch)
        {
            return ch == '"';
        }
    }
}