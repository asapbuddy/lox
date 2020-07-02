using System;

namespace Cslox
{
    internal abstract class Token
    {
        private readonly string _lexem;
        private readonly Object _literal;

        protected Token(string lexem, object literal)
        {
            _lexem = lexem;
            _literal = literal;
        }

        public override string ToString()
        {
            return $"{_lexem} {_literal}";
        }
    }

    internal interface IToken
    {
        public IToken Generate(in ForwardIterator it);
    }
}