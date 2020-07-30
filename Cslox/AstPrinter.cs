namespace Cslox
{
    public class AstPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr) => expr.accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return parenthesize(expr.Oper.Lexem, expr.Left, expr.Right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return parenthesize("group", expr.Expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value == null ? "nil" : expr.value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return parenthesize(expr.Oper.Lexem, expr.Right);
        }


        private string parenthesize(string name, params Expr[] exprs)
        {
            var result = $"({name}";
            foreach (var expr in exprs)
                result += $" {expr.accept(this)}";
            result += ")";
            return result;
        }
    }
}