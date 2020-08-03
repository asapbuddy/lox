namespace Cslox
{
    public class AstPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr) => expr.accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.Oper.Lexem, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value == null ? "nil" : expr.value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.Oper.Lexem, expr.Right);
        }


        private string Parenthesize(string name, params Expr[] exprs)
        {
            var result = $"({name}";
            foreach (var expr in exprs)
                result += $" {expr.accept(this)}";
            result += ")";
            return result;
        }
    }
}