using System;

namespace Cslox
{
    internal class Token
    {
        private readonly TokenType _type;
        private readonly string _lexem;
        private readonly Object _literal;
        private readonly int _line;

        public Token(TokenType type, string lexem, object literal, int line)
        {
            _type = type;
            _lexem = lexem;
            _literal = literal;
            _line = line;
        }

        public override string ToString() => $"{_type} {_lexem} {_literal}";
    }
}