using System.Data;

namespace Lox;

class Interpreter : Expr.IVisitor<object>
{
    private object Evaluate(Expr expr)
    {
        return expr.Accept(this);
    }

    public void Interpret(Expr expr)
    {
        try
        {
            object value = Evaluate(expr);
            Console.WriteLine(Stringify(value));
        }
        catch (RunTimeError error)
        {
            Lox.RuntimeError(error);
        }
    }

    public object VisitBinaryExpr(Expr.Binary expr)
    {
        object left = Evaluate(expr.left);
        object right = Evaluate(expr.right);

        switch (expr.opp.type)
        {
            case TokenType.BANG_EQUAL: return !IsEqual(left, right);
            case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
            case TokenType.GREATER:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left >= (double)right;
            case TokenType.LESS:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left < (double)right;
            case TokenType.LESS_EQUAL:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left <= (double)right;
            case TokenType.MINUS:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left - (double)right;
            case TokenType.SLASH:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left / (double)right;
            case TokenType.STAR:
                CheckNumberOperands(expr.opp, left, right);
                return (double)left * (double)right;
            case TokenType.PLUS:
                if (left is double && right is double)
                {
                    return (double)left + (double)right;
                }

                if (left is string && right is string)
                {
                    return (string)left + (string)right;
                }

                throw new RunTimeError(expr.opp, "Operands must be two numbers or two string.");
        }


        return null;
    }

    public object VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.value;
    }

    public object VisitGroupingExpr(Expr.Grouping expr)
    {
        return Evaluate(expr.expr);
    }

    public object VisitUnaryExpr(Expr.Unary expr)
    {
        object right = Evaluate(expr.expr);

        switch (expr.opp.type)
        {
            case TokenType.BANG:
                return !IsTruthy(right);
            case TokenType.MINUS:
                CheckNumberOperand(expr.opp, right);
                return -(double)right;
        }



        return null;
    }

    private void CheckNumberOperand(Token opp, object operand)
    {
        if (opp is double) return;
        throw new RunTimeError(opp, "Operand must be a number.");
    }

    private void CheckNumberOperands(Token opp, object left, object right)
    {
        if (left is double && right is double) return;
        
        throw new RunTimeError(opp, "Operands must be numbers.");
    }

    private static bool IsTruthy(object obj)
    {
        if (obj == null) return false;
        if (obj is bool v) return v;
        return true;
    }

    private bool IsEqual(object a, object b)
    {
        if (a == null && b == null) return true;
        if (a == null) return false;

        return a.Equals(b);
    }

    private string Stringify(object obj)
    {
        if (obj == null) return "nil";

        if (obj is double)
        {
            string text = obj.ToString();
            if (text.EndsWith(".0"))
            {
                text = text[.. (text.Length - 2)];
            }
            return text;
        }

        return obj.ToString();
    }
}