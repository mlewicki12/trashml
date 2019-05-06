
namespace TrashML
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitUnaryExpr(Unary expr);
            R VisitVariableExpr(Variable expr);
            R VisitLiteralExpr(Literal expr);
        }

        public class Binary : Expr
        {
            public readonly Expr Left;
            public readonly Lexer.Token Operator;
            public readonly Expr Right;

            public Binary(Expr left, Lexer.Token opr, Expr right)
            {
                Left = left;
                Operator = opr;
                Right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Grouping : Expr
        {
            public readonly Expr Expression;

            public Grouping(Expr expression)
            {
                Expression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }

        public class Unary : Expr
        {
            public readonly Lexer.Token Operator;
            public readonly Expr Right;

            public Unary(Lexer.Token op, Expr right)
            {
                Operator = op;
                Right = right;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

        public class Variable : Expr
        {
            public readonly Lexer.Token Name;

            public Variable(Lexer.Token name)
            {
                Name = name;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVariableExpr(this);
            }
        }

        public class Literal : Expr
        {
            public readonly object Value;

            public Literal(object value)
            {
                Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}
