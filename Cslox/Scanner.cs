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
        private readonly List<string> _errors;

        public Scanner(in string source)
        {
            _source = source;
            _errors = new List<string>();
            InitKeywords();
        }

        public IEnumerable<string> GetErrors()
        {
            return _errors;
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
                try
                {
                    _start = _current;
                    ScanToken();
                }
                catch (Exception e)
                {
                    _errors.Add($"[Scanning Error] {e.Message} at line: {_line}\n {_source.Split('\n')[_line - 1]}");
                }
            }

            _tokens.Add(new Token(TokenType.EOF, "", null));
            return _tokens;
        }

        private void ScanToken()
        {
            var ch = Advance();

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
                        break;
                    }
                    else if (Match('*'))
                    {
                        do
                        {
                            Advance();
                            if (Peek() == '\n')
                            {
                                ++_line;
                                continue;
                            }

                            if (Peek() == '*')
                            {
                                Advance();
                                if (Peek() == '/')
                                {
                                    Advance();
                                    return;
                                }
                            }
                        } while (!IsAtEnd());

                        throw new Exception("Multiline comments must be closed");
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
                        throw new Exception("Unexpected symbol");
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
                throw new Exception("Unexpected symbol");
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

        private bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private void CreateString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;
                Advance();
            }

            if (IsAtEnd())
            {
                throw new Exception("Unterminated string");
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

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}