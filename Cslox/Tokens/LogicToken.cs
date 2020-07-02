namespace Cslox
{
    internal class LogicToken : IToken
    {
        private readonly LogicSign _type;

        public override string ToString()
        {
            return $"{_type} {base.ToString()}";
        }

        public IToken Generate(in ForwardIterator it)
        {
            if (it.Match('='))
                type += 1;
            throw new System.NotImplementedException();
        }
    }
}