
namespace TrashML
{
    public abstract class Expr
    {
        public interface Visitor<R>
        {
            R VisitBinaryExpr(Binary expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitUnaryExpr(Unary expr);
            R VisitVariableExpr(Variable expr);
            R VisitLiteralExpr(Literal expr);
        }

        public class Binary : Expr
        {
            public Expr Left;
            public Lexer.Token Operator;
            public Expr Right;

            public Binary(Expr left, Lexer.Token opr, Expr right)
            {
                this.Left = left;
                this.Operator = opr;
                this.Right = right;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Grouping : Expr
        {
            public Expr Expression;

            public Grouping(Expr expression)
            {
                this.Expression = expression;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitGroupingExpr(this);
            }
        }

        public class Unary : Expr
        {
            public Lexer.Token Operator;
            public Expr Right;

            public Unary(Lexer.Token oprt, Expr right)
            {
                this.Operator = oprt;
                this.Right = right;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitUnaryExpr(this);
            }
        }

        public class Variable : Expr
        {
            public Lexer.Token Name;

            public Variable(Lexer.Token name)
            {
                this.Name = name;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitVariableExpr(this);
            }
        }

        public class Literal : Expr
        {
            public object Value;

            public Literal(object value)
            {
                this.Value = value;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public abstract R Accept<R>(Visitor<R> visitor);
    }
}
