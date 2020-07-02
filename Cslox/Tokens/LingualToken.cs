namespace Cslox
{
    internal class LingualToken : IToken
    {
        private readonly LingualSign _type;

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }

        public IToken Generate(in ForwardIterator it)
        {
            throw new System.NotImplementedException();
        }
    }
}