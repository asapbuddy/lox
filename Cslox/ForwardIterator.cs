namespace Cslox
{
    public class ForwardIterator
    {
        private readonly string _source;
        private int _start = 0, _current = 0, _line = 1;

        public ForwardIterator(in string source)
        {
            _source = source;
        }

        public string CurrentLexem()
        {
            return _source.Substring(_start, _current - _start);
        }

        public char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        public char Peek()
        {
            return IsAtEnd() ? '\0' : _source[_current];
        }

        public bool Match(char expected)
        {
            if (IsAtEnd() || _source[_current] != expected)
                return false;
            _current++;
            return true;
        }

        public char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        public bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}