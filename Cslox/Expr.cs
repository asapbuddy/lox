namespace Cslox
{
    public abstract class Expr
    {
        public interface IVisitor<T>
        {
            T VisitBinaryExpr(Binary expr);
            T VisitGroupingExpr(Grouping expr);
            T VisitLiteralExpr(Literal expr);
            T VisitUnaryExpr(Unary expr);
        }

        public abstract T accept<T>(IVisitor<T> visitor);

        public class Binary : Expr
        {
            public Binary(Expr Left, Token Oper, Expr Right)
            {
                this.Left = Left;
                this.Oper = Oper;
                this.Right = Right;
            }

            public override T accept<T>(IVisitor<T> visitor) => visitor.VisitBinaryExpr(this);
            public Expr Left;
            public Token Oper;
            public Expr Right;
        }

        public class Grouping : Expr
        {
            public Grouping(Expr Expression)
            {
                this.Expression = Expression;
            }

            public override T accept<T>(IVisitor<T> visitor) => visitor.VisitGroupingExpr(this);
            public Expr Expression;
        }

        public class Literal : Expr
        {
            public Literal(object value)
            {
                this.value = value;
            }

            public override T accept<T>(IVisitor<T> visitor) => visitor.VisitLiteralExpr(this);
            public object value;
        }

        public class Unary : Expr
        {
            public Unary(Token Oper, Expr Right)
            {
                this.Oper = Oper;
                this.Right = Right;
            }

            public override T accept<T>(IVisitor<T> visitor) => visitor.VisitUnaryExpr(this);
            public Token Oper;
            public Expr Right;
        }
    }
}