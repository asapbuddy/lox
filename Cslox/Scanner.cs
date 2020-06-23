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

            TokenType tokenType = TokenType.Unexpected;
            switch (ch)
            {
                case '(':
                    tokenType = TokenType.LEFT_PAREN;
                    break;
                case ')':
                    tokenType = TokenType.RIGHT_PAREN;
                    break;
                case '{':
                    tokenType = TokenType.LEFT_BRACE;
                    break;
                case '}':
                    tokenType = TokenType.RIGHT_BRACE;
                    break;
                case ',':
                    tokenType = TokenType.COMMA;
                    break;
                case '.':
                    tokenType = TokenType.DOT;
                    break;
                case '-':
                    tokenType = TokenType.MINUS;
                    break;
                case '+':
                    tokenType = TokenType.PLUS;
                    break;
                case ';':
                    tokenType = TokenType.SEMICOLON;
                    break;
                case '*':
                    tokenType = TokenType.STAR;
                    break;
                case '!':
                    tokenType = Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG;
                    break;
                case '=':
                    tokenType = Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL;
                    break;
                case '<':
                    tokenType = Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS;
                    break;
                case '>':
                    tokenType = Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER;
                    break;
                case '/':
                {
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                            Advance();
                        return;
                    }

                    tokenType = TokenType.SLASH;


                    break;
                }
                case '"':
                    CreateString();
                    break;

                case ' ':
                case '\r':
                case '\t':
                    return;
                case '\n':
                    _line++;
                    break;
                default:
                    if (IsDigit(ch))
                    {
                        CreateNumber();
                    }
                    else
                    {
                        tokenType = TokenType.Unexpected;
                    }

                    break;
            }

            if (tokenType == TokenType.Unexpected)
                Error.ReportError(_source.Substring(_start, _current - _start), _line, _current,
                    "Unexpected symbol");
            else
                AddToken(tokenType);
        }

        private void CreateNumber()
        {
            while (IsDigit(Peek()))
                Advance();
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }

            if (double.TryParse(_source.Substring(_start, _current - _start), out var literal))
                AddToken(TokenType.NUMBER, literal);
            else
                Error.ReportError(_source.Substring(_start, _current - _start), _line, _current,
                    "Unexpected symbol");
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private bool IsDigit(char ch) => ch >= '0' && ch <= '9';


        private void CreateString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Error.ReportError(_source.Substring(_start, _source.IndexOf('\n', _start) - _start), _line, _current,
                    "Unterminated string");
                return;
            }

            Advance();
            var value = _source.Substring(_start + 1, _current - _start);
            AddToken(TokenType.STRING, value);
        }

        private char Peek()
        {
            return IsAtEnd() ? '\0' : _source[_current];
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