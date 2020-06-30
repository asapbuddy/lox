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

    internal class SyntaxToken : Token
    {
        private readonly SyntaxSign _type;

        public SyntaxToken(SyntaxSign type, string lexem, object literal) : base(lexem, literal)
        {
            _type = type;
        }

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }
    }

    internal class LogicToken : Token
    {
        private readonly LogicSign _type;

        public LogicToken(LogicSign type, string lexem, object literal) : base(lexem, literal)
        {
            _type = type;
        }

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }
    }

    internal class LexicalToken : Token
    {
        private readonly LexicalSign _type;

        public LexicalToken(LexicalSign type, string lexem, object literal) : base(lexem, literal)
        {
            _type = type;
        }

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }
    }

    internal class LingualToken : Token
    {
        private readonly LingualSign _type;

        public LingualToken(LingualSign type, string lexem, object literal) : base(lexem, literal)
        {
            _type = type;
        }

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }
    }
}