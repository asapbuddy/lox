using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cslox
{
    internal sealed class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int _start = 0, _current = 0, _line = 1;
        private readonly LookupTable _table;

        public Scanner(in string source)
        {
            _source = source;
            _table = new LookupTable();
        }


        public IEnumerable<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new LingualToken(LingualSign.EOF, "", null));
            return _tokens;
        }

        private void ScanToken()
        {
            var ch = Advance();

            var symbolKind = _table.GetSymbolKind(ch);

            if (symbolKind == SymbolKind.SyntaxSign)
            {
                _tokens.Add(new SyntaxToken(_table.GetSyntaxToken(ch), CurrentLexem(), null));
            }
            else if (symbolKind == SymbolKind.LogicSign)
            {
                var type = _table.GetLogicToken(ch);
                if (Match('='))
                    type += 1;
                _tokens.Add(new LogicToken(type, CurrentLexem(), null));
            }
            else if (symbolKind == SymbolKind.LexicalSign)
            {
                
            }


            if (_table.IsSpecial(ch))
            {
                if (!_table.IsDelimiter(ch))
                {
                }
            }

            CreateSpecial(ch);
            else if (_table.IsDelimiter(ch))
                return;
            else if (ch == '/' && Match('/') || ch == '/' && Match('*'))
                ProcessComment();

            else if (ch == '"')
                CreateString();
            else if (IsAlpha(ch))
                CreateIdentifier();
            else if (IsDigit(ch))
                CreateNumber();
            else
                Error.ReportError();


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
                        Error.ReportError(CurrentLexem(), _line, _current,
                            "Unexpected symbol");
                    }

                    break;
            }
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
                //TODO fix that error lines
                Error.ReportError(_source.Substring(_start, _source.IndexOf('\n', _start) - _start), _line, _current,
                    "Unterminated string");
                return;
            }

            Advance();
            AddToken(TokenType.STRING, CurrentLexem());
        }

        private void CreateIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
                Advance();

            AddToken(_keywords.TryGetValue(CurrentLexem(), out var type) ? type : TokenType.IDENTIFIER);
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

        private void AddToken<T>(T token, Object literal = null)
        {
            //_tokens.Add(new T(token, CurrentLexem(), literal));
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