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
        private readonly List<string> _errors;
        private int _start, _current, _line;

        public Scanner(in string source)
        {
            _source = source;
            _errors = new List<string>();
            InitKeywords();
        }

        public IReadOnlyList<string> GetErrors()
        {
            return _errors;
        }

        private void InitKeywords()
        {
            _keywords.Add("and", TokenType.And);
            _keywords.Add("class", TokenType.Class);
            _keywords.Add("else", TokenType.Else);
            _keywords.Add("false", TokenType.False);
            _keywords.Add("for", TokenType.For);
            _keywords.Add("fun", TokenType.Fun);
            _keywords.Add("if", TokenType.If);
            _keywords.Add("nil", TokenType.Nil);
            _keywords.Add("or", TokenType.Or);
            _keywords.Add("print", TokenType.Print);
            _keywords.Add("return", TokenType.Return);
            _keywords.Add("super", TokenType.Super);
            _keywords.Add("this", TokenType.This);
            _keywords.Add("true", TokenType.True);
            _keywords.Add("var", TokenType.Var);
            _keywords.Add("while", TokenType.While);
        }

        public IReadOnlyList<Token> ScanTokens()
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
                    _errors.Add($"[Scanning Error] {e.Message} at line: {_line + 1}\n {_source.Split('\n')[_line]}");
                }
            }

            _tokens.Add(new Token(TokenType.Eof, "", null));
            return _tokens;
        }

        private void ScanToken()
        {
            var ch = Advance();

            switch (ch)
            {
                case '(':
                    AddToken(TokenType.LeftParen);
                    break;
                case ')':
                    AddToken(TokenType.RightParen);
                    break;
                case '{':
                    AddToken(TokenType.LeftBrace);
                    break;
                case '}':
                    AddToken(TokenType.RightBrace);
                    break;
                case ',':
                    AddToken(TokenType.Comma);
                    break;
                case '.':
                    AddToken(TokenType.Dot);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case ';':
                    AddToken(TokenType.Semicolon);
                    break;
                case '*':
                    AddToken(TokenType.Star);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                    break;
                case '/':
                {
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                            Advance();
                        break;
                    }

                    if (Match('*'))
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

                    AddToken(TokenType.Slash);
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

            AddToken(_keywords.TryGetValue(CurrentLexem(), out var type) ? type : TokenType.Identifier);
        }

        private static bool IsAlphaNumeric(char ch)
        {
            return IsAlpha(ch) || IsDigit(ch);
        }

        private static bool IsAlpha(char ch)
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
                AddToken(TokenType.Number, literal);
            else
                throw new Exception("Unexpected symbol");
        }

        private string CurrentLexem()
        {
            return _source.Substring(_start, _current - _start);
        }

        private char PeekNext()
        {
            return _current + 1 >= _source.Length ? '\0' : _source[_current + 1];
        }

        private static bool IsDigit(char ch)
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
            AddToken(TokenType.String, value);
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

        private void AddToken(TokenType type, object literal = null)
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