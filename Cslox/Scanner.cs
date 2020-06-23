using System;
using System.Collections.Generic;

namespace Cslox
{
    internal sealed class Scanner
    {
        private readonly string _source;
        private List<Token> _tokens = new List<Token>();

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
            var type = ch switch
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
                _ => TokenType.EOF
            };

            AddToken(type);
        }

        void AddToken(TokenType type, Object literal = null)
        {
            var text = _source.Substring(_start, _current);
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