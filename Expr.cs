namespace Lox;

abstract class Expr
{
    public interface IVisitor<T>
    {
        T VisitBinaryExpr(Binary expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitUnaryExpr(Unary expr);
    }

    public class Binary : Expr
    {
        public readonly Expr left;
        public readonly Token opp;
        public readonly Expr right;

        public Binary(Expr left, Token opp, Expr right)
        {
            this.left = left;
            this.opp = opp;
            this.right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public class Grouping : Expr
    {
        public readonly Expr expr;

        public Grouping(Expr expr)
        {
            this.expr = expr;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }
    }

    public class Literal : Expr
    {
        public readonly object value;

        public Literal(object value)
        {
            this.value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }

    public class Unary : Expr
    {
        public readonly Expr expr;
        public readonly Token opp;

        public Unary(Token opp, Expr expr)
        {
            this.expr = expr;
            this.opp = opp;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
}