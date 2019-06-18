
using System.Collections.Generic;

namespace TrashML
{
    public abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitDefineStmt(Define stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitMemberStmt(Member stmt);
            R VisitLetStmt(Assign stmt);
            R VisitPrintStmt(Print stmt);
            R VisitRepeatStmt(Repeat stmt);
            R VisitReturnStmt(Return stmt);
            R VisitRequireStmt(Require stmt);
            R VisitIfStmt(If stmt);
            R VisitMacroStmt(Macro stmt);
            R VisitClassStmt(Class stmt);
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

        public class Define : Stmt
        {
            public readonly List<Member> Statements;

            public Define(List<Member> statements)
            {
                Statements = statements;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitDefineStmt(this);
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

        public class Member : Stmt
        {
            public readonly Lexer.Token Name;
            
            public readonly Expr Initialiser;
            public readonly Block Body;

            public Member(Lexer.Token name, Expr initializer)
            {
                Name = name;
                Initialiser = initializer;
            }

            public Member(Lexer.Token name, Block body)
            {
                Name = name;
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitMemberStmt(this);
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

            public Repeat(Expr cond, Block bl)
            {
                Condition = cond;
                Block = bl;
            }

            public Repeat(Expr lv, Expr hv, Block bl)
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

        public class Return : Stmt
        {
            public readonly Expr Value;

            public Return(Expr val)
            {
                Value = val;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitReturnStmt(this);
            }
        }

        public class Require : Stmt 
        {
            public readonly Expr File;

            public Require(Expr file) 
            {
                File = file;
            }

            public override R Accept<R>(IVisitor<R> visitor) 
            {
                return visitor.VisitRequireStmt(this);
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

            public Macro(Lexer.Token name, Block body)
            {
                Name = name;
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitMacroStmt(this);
            }
        }

        public class Class : Stmt
        {
            public readonly Lexer.Token Name;
            public readonly Define Body;

            public Class(Lexer.Token name, Define body)
            {
                Name = name;
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitClassStmt(this);
            }
        }

        public abstract R Accept<R>(IVisitor<R> visitor);
    }
}