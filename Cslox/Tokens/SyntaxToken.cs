using System;

namespace Cslox
{
    internal class SyntaxToken : IToken
    {
        private readonly SyntaxSign _type;

        public SyntaxToken()
        {
        }

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }

        public IToken Generate(in ForwardIterator it)
        {
            if (it.Peek() == '/' && it.Match('/'))
            {
                while (it.Peek() != '\n' && !it.IsAtEnd())
                    it.Advance();
            }
            else 
            {
                _tokens.Add(new SyntaxToken(type, CurrentLexem(), null));
            }
        }
    }
}