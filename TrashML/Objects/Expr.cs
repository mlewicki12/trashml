
using System.Collections.Generic;
using TrashML.Main;

namespace TrashML.Objects
{
    public abstract class Expr
    {
        public interface IVisitor<R>
        {
            R VisitArgExpr(Arg expr);
            R VisitBinaryExpr(Binary expr);
            R VisitGroupingExpr(Grouping expr);
            R VisitLiteralExpr(Literal expr);
            R VisitNewExpr(New expr);
            R VisitUnaryExpr(Unary expr);
            R VisitVariableExpr(Variable expr);
        }

        public abstract R Accept<R>(IVisitor<R> visitor);

        public class Arg : Expr
        {
            public readonly List<Stmt.Assign> Values;

            public Arg(List<Stmt.Assign> args)
            {
                Values = args;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitArgExpr(this);
            }
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

        public class Literal : Expr
        {
            public readonly TrashObject Value;

            public Literal(TrashObject value)
            {
                Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public class New : Expr
        {
            public readonly Lexer.Token Identifier;
            public readonly Arg Arguments;

            public New(Lexer.Token id, Arg args = null)
            {
                Identifier = id;
                Arguments = args;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitNewExpr(this);
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
            public readonly Expr.Arg Arguments;

            public Variable(Lexer.Token name, Expr.Arg args)
            {
                Name = name;
                Arguments = args;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitVariableExpr(this);
            }
        }
    }
}
