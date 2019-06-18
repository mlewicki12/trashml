
using System;
using System.Collections.Generic;
using TrashML.Elements;

namespace TrashML
{
    // might possibly extrapolate into ParseHelp as well
    // actually I'm gonna do that later today
    public class Interpreter : Expr.IVisitor<object>,
        Stmt.IVisitor<string> // this string doesn't actually matter, but we can't use Void in C#
    {
        public class RuntimeError : Exception
        {
            public RuntimeError(string msg) : base(msg)
            {
            }
        }

        public Environment IntEnvironment;
        public List<RuntimeError> Errors;

        public Interpreter()
        {
            IntEnvironment = new Environment("TML Interpreter", null);
            Errors = new List<RuntimeError>();
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (var stmt in statements)
                {
                    Execute(stmt);
                }
            }
            catch (RuntimeError e)
            {
                Errors.Add(e);
            }
        }

        public bool Error()
        {
            return Errors.Count != 0;
        }

        public object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        public void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public void ExecuteBlock(List<Stmt> statements, Environment env)
        {
            Environment previous = IntEnvironment;

            try
            {
                IntEnvironment = env;

                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                IntEnvironment = previous;
            }
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            return this.BinaryExpr(expr);
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return this.GroupingExpr(expr);
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            return this.UnaryExpr(expr);
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            return this.VariableExpr(expr);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public string VisitBlockStmt(Stmt.Block stmt)
        {
            return this.BlockStmt(stmt);
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.InnerExpression);
            return "";
        }

        public string VisitClassStmt(Stmt.Class stmt)
        {
            return this.ClassStmt(stmt);
        }

        public string VisitDefineStmt(Stmt.Define stmt)
        {
            return this.DefineStmt(stmt);
        }

        public string VisitMemberStmt(Stmt.Member stmt)
        {
            return this.MemberStmt(stmt);
        }

        public string VisitReturnStmt(Stmt.Return stmt)
        {
            return this.ReturnStmt(stmt);
        }

        public string VisitAssignStmt(Stmt.Assign stmt)
        {
            return this.AssignStmt(stmt);
        }

        public string VisitPrintStmt(Stmt.Print stmt)
        {
            return this.PrintStmt(stmt);
        }

        public string VisitRepeatStmt(Stmt.Repeat stmt)
        {
            return this.RepeatStmt(stmt);
        }

        public string VisitRequireStmt(Stmt.Require stmt)
        {
            return this.RequireStmt(stmt);
        }

        public string VisitIfStmt(Stmt.If stmt)
        {
            return this.IfStmt(stmt);
        }

        public string VisitMacroStmt(Stmt.Macro stmt)
        {
            return this.MacroStmt(stmt);
        }
    }

}