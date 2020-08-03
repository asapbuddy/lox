using System.Collections;
using System.Collections.Generic;

namespace Cslox
{
    public class RpnPrinter : Expr.IVisitor<string>
    {
        private Stack<string> _stack = new Stack<string>();

        public string Print(Expr expr) => expr.accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.Oper.Lexem, expr.Left, new Expr.Literal(" "), expr.Right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return expr.Expression.accept(this);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value == null ? "nil" : expr.value.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return $"{expr.Oper}";
        }

        private string Parenthesize(string name, params Expr[] exprs)
        {
            var result = "";
            foreach (var expr in exprs)
            {
                result += $"{expr.accept(this)}";
            }

            result += $" {name}";


            return result;
        }
    }
}