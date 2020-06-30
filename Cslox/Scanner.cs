using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cslox
{
    internal sealed class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>();
        private int _start = 0, _current = 0, _line = 1;

        public Scanner(in string source)
        {
            _source = source;
            InitKeywords();
        }

        private void InitKeywords()
        {
            _keywords.Add("and", TokenType.AND);
            _keywords.Add("class", TokenType.CLASS);
            _keywords.Add("else", TokenType.ELSE);
            _keywords.Add("false", TokenType.FALSE);
            _keywords.Add("for", TokenType.FOR);
            _keywords.Add("fun", TokenType.FUN);
            _keywords.Add("if", TokenType.IF);
            _keywords.Add("nil", TokenType.NIL);
            _keywords.Add("or", TokenType.OR);
            _keywords.Add("print", TokenType.PRINT);
            _keywords.Add("return", TokenType.RETURN);
            _keywords.Add("super", TokenType.SUPER);
            _keywords.Add("this", TokenType.THIS);
            _keywords.Add("true", TokenType.TRUE);
            _keywords.Add("var", TokenType.VAR);
            _keywords.Add("while", TokenType.WHILE);
        }

        public IEnumerable<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null));
            return _tokens;
        }

        private void ScanToken()
        {
            var ch = Advance();

            TokenType tokenType = TokenType.Unexpected;
            switch (ch)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                {
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                            Advance();
                        return;
                    }

                    AddToken(TokenType.SLASH);
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
                    return;
                default:
                    if (IsDigit(ch))
                    {
                        CreateNumber();
                    }
                    else if (IsAlpha(ch))
                    {
                        CreateIdentifier();
                    }
                    else
                    {
                        if (tokenType == TokenType.Unexpected)
                            Error.ReportError(_source.Substring(_start, _current - _start), _line, _current,
                                "Unexpected symbol");
                    }

                    break;
            }
        }

        private void CreateIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
                Advance();

            AddToken(_keywords.TryGetValue(CurrentLexem(), out var type) ? type : TokenType.IDENTIFIER);
        }

        private bool IsAlphaNumeric(char ch)
        {
            return IsAlpha(ch) || IsDigit(ch);
        }

        private bool IsAlpha(char ch)
        {
            return ch >= 'a' && ch <= 'z' ||
                   ch >= 'A' && ch <= 'Z' ||
                   ch == '_';
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


            if (double.TryParse(CurrentLexem(), NumberStyles.Number, CultureInfo.InvariantCulture, out var literal))
                AddToken(TokenType.NUMBER, literal);
            else
                Error.ReportError(CurrentLexem(), _line, _current,
                    "Unexpected symbol");
        }

        private string CurrentLexem()
        {
            return _source.Substring(_start, _current - _start);
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
                //TODO fix that error lines
                Error.ReportError(_source.Substring(_start, _source.IndexOf('\n', _start) - _start), _line, _current,
                    "Unterminated string");
                return;
            }

            Advance();
            var value = _source.Substring(_start, _current - _start);
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
            _tokens.Add(new Token(type, text, literal));
        }

        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        private bool IsAtEnd() => _current >= _source.Length;
    }
}