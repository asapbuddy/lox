namespace Cslox
{
    public class RpnPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr) => expr.accept(this);

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            throw new System.NotImplementedException();
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            throw new System.NotImplementedException();
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            throw new System.NotImplementedException();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            throw new System.NotImplementedException();
        }
    }
}