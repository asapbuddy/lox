using System;

namespace Cslox
{
    internal class Token
    {
        private readonly TokenType _type;
        private readonly string _lexem;
        private readonly Object _literal;

        public Token(TokenType type, string lexem, object literal)
        {
            _type = type;
            _lexem = lexem;
            _literal = literal;
        }

        public override string ToString() => $"{_type} {_lexem}";
    }
}