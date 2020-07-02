using System;

namespace Cslox
{
    internal class LexicalToken : IToken
    {
        private readonly LexicalSign _type;

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }

        public IToken Generate(in ForwardIterator it)
        {
            if (it.Peek() == '"')
            {
                while (it.Peek() != '"' && !it.IsAtEnd())
                {
                    //TODO figure out of lines
                    //if (Peek() == '\n') _line++;
                    it.Advance();
                }

                if (it.IsAtEnd())
                {
                    //TODO fix that error lines
                    //Error.ReportError(_source.Substring(_start, _source.IndexOf('\n', _start) - _start), _line,
                    //    _current,
                    //    "Unterminated string");
                    throw new Exception("Unterminated string");
                }

                it.Advance();
                return new LexicalToken();
                _tokens.Add(new LexicalToken(LexicalSign.STRING, CurrentLexem(), null));
            }
            
        }
    }
}