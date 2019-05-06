
using System.Collections.Generic;

namespace TrashML
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitLetStmt(Assign stmt);
            R VisitPrintStmt(Print stmt);
            R VisitRepeatStmt(Repeat stmt);
            R VisitDottedStmt(Dotted stmt);
            R VisitIfStmt(If stmt);
            R VisitMacroStmt(Macro stmt);
        }

        public class Block : Stmt
        {
            public readonly List<Stmt> Statements;

            public Block(List<Stmt> statements)
            {
                Statements = statements;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }
        }

        public class Expression : Stmt
        {
            public readonly Expr InnerExpression;

            public Expression(Expr expression)
            {
                InnerExpression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }
        }

        public class Assign : Stmt
        {
            public readonly Lexer.Token Name;
            public readonly Expr Initialiser;

            public Assign(Lexer.Token name, Expr initializer)
            {
                Name = name;
                Initialiser = initializer;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitLetStmt(this);
            }
        }

        public class Print : Stmt
        {
            public readonly Expr Expression;

            public Print(Expr expr)
            {
                Expression = expr;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }
        }

        public class Repeat : Stmt
        {
            public readonly Expr LowValue;
            public readonly Expr HighValue;
            public readonly Expr Condition;
            public readonly Block Block;

            public Repeat(Expr cond, Stmt.Block bl)
            {
                Condition = cond;
                Block = bl;
            }

            public Repeat(Expr lv, Expr hv, Stmt.Block bl)
            {
                LowValue = lv;
                HighValue = hv;
                Block = bl;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitRepeatStmt(this);
            }
        }

        public class Dotted : Stmt
        {
            public readonly Lexer.Token Operand;
            public readonly Lexer.Token Operation;
            public readonly Expr Arguments;

            public Dotted(Lexer.Token operand, Lexer.Token operation, Expr arg)
            {
                Operand = operand;
                Operation = operation;
                Arguments = arg;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitDottedStmt(this);
            }
        }

        public class If : Stmt
        {
            public readonly Expr Condition;
            public readonly Stmt WhenTrue;
            public readonly Stmt WhenFalse;

            public If(Expr cond, Stmt tr, Stmt fl)
            {
                Condition = cond;
                WhenTrue = tr;
                WhenFalse = fl;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }

        }

        public class Macro : Stmt
        {
            public readonly Lexer.Token Name;
            public readonly Block Body;

            public Macro(Lexer.Token name, Stmt.Block body)
            {
                Name = name;
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitMacroStmt(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}