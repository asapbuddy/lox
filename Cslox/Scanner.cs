using System;
using System.Collections.Generic;

namespace Cslox
{
    internal sealed class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();

        private int _start = 0, _current = 0, _line = 1;

        public Scanner(in string source)
        {
            _source = source;
        }

        public IEnumerable<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line));
            return _tokens;
        }

        private void ScanToken()
        {
            var ch = Advance();
            var tokenType = ch switch
            {
                '(' => TokenType.LEFT_PAREN,
                ')' => TokenType.RIGHT_PAREN,
                '{' => TokenType.LEFT_BRACE,
                '}' => TokenType.RIGHT_BRACE,
                ',' => TokenType.COMMA,
                '.' => TokenType.DOT,
                '-' => TokenType.MINUS,
                '+' => TokenType.PLUS,
                ';' => TokenType.SEMICOLON,
                '*' => TokenType.STAR,
                '!' => Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG,
                '=' => Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL,
                '<' => Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS,
                '>' => Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER,


                _ => TokenType.Unexpected
            };

            if (tokenType == TokenType.Unexpected)
                Error.ReportError(_line, _current, "Unexpected symbol");
            else
                AddToken(tokenType);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd() || _source[_current] != expected)
                return false;

            _current++;
            return true;
        }

        void AddToken(TokenType type, Object literal = null)
        {
            var text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        private bool IsAtEnd() => _current >= _source.Length;
    }
}