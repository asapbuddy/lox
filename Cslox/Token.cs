namespace Cslox
{
    public class Token
    {
        public TokenType Type;
        public string Lexem;
        public object Literal;

        public Token(TokenType type, string lexem, object literal)
        {
            Type = type;
            Lexem = lexem;
            Literal = literal;
        }

        public override string ToString() => $"{Type} {Lexem} ";
    }
}