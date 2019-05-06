
using System.Collections.Generic;

namespace TrashML
{
    public abstract class Stmt
    {
        public interface Visitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitAssignStmt(Assign stmt);
            R VisitPrintStmt(Print stmt);
            R VisitRepeatStmt(Repeat stmt);
            R VisitDottedStmt(Dotted stmt);
            R VisitIfStmt(If stmt);
            R VisitMacroStmt(Macro stmt);
        }

        public class Block : Stmt
        {
            public List<Stmt> Statements;

            public Block(List<Stmt> statements)
            {
                this.Statements = statements;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }
        }

        public class Expression : Stmt
        {
            public Expr InnerExpression;

            public Expression(Expr expression)
            {
                this.InnerExpression = expression;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }
        }

        public class Assign : Stmt
        {
            public Lexer.Token Name;
            public Expr Initialiser;

            public Assign(Lexer.Token name, Expr initializer)
            {
                this.Name = name;
                this.Initialiser = initializer;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitAssignStmt(this);
            }
        }

        public class Print : Stmt
        {
            public Expr Expression;

            public Print(Expr expr)
            {
                this.Expression = expr;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }
        }

        public class Repeat : Stmt
        {
            public Expr LowValue;
            public Expr HighValue;
            public Expr Condition;
            public Stmt.Block Block;

            public Repeat(Expr cond, Stmt.Block bl)
            {
                this.Condition = cond;
                this.Block = bl;
            }

            public Repeat(Expr lv, Expr hv, Stmt.Block bl)
            {
                this.LowValue = lv;
                this.HighValue = hv;
                this.Block = bl;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitRepeatStmt(this);
            }
        }

        public class Dotted : Stmt
        {
            public Lexer.Token Operation;
            public Expr Arguments;

            public Dotted(Lexer.Token op, Expr arg)
            {
                this.Operation = op;
                this.Arguments = arg;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitDottedStmt(this);
            }
        }

        public class If : Stmt
        {
            public Expr Condition;
            public Stmt WhenTrue;
            public Stmt WhenFalse;

            public If(Expr cond, Stmt tr, Stmt fl)
            {
                this.Condition = cond;
                this.WhenTrue = tr;
                this.WhenFalse = fl;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }

        }

        public class Macro : Stmt
        {
            public Lexer.Token Name;
            public Stmt.Block Body;

            public Macro(Lexer.Token name, Stmt.Block body)
            {
                this.Name = name;
                this.Body = body;
            }

            public override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitMacroStmt(this);
            }
        }

        public abstract R Accept<R>(Visitor<R> visitor);
    }
}