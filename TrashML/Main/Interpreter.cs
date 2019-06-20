
using System;
using System.Collections.Generic;
using TrashML.Elements;
using TrashML.Objects;
using TrashML.Objects.Overrides;

namespace TrashML.Main
{
    public class Interpreter : Expr.IVisitor<TrashObject>,
        Stmt.IVisitor<TrashObject> // this string doesn't actually matter, but we can't use Void in C#
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

            NumberOverrides.AddOverrides();
            BooleanOverrides.AddOverrides();
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

        public TrashObject Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        public TrashObject Execute(Stmt stmt)
        {
            return stmt.Accept(this);
        }

        public TrashObject ExecuteBlock(List<Stmt> statements, Environment env)
        {
            Environment previous = IntEnvironment;

            try
            {
                IntEnvironment = env;

                foreach (var statement in statements)
                {
                    var value = Execute(statement);
                    if (value != null) // we must've hit a return
                    {
                        IntEnvironment = previous;
                        return value;
                    }
                }
            }
            finally
            {
                IntEnvironment = previous;
            }

            return null;
        }

        public TrashObject VisitBinaryExpr(Expr.Binary expr)
        {
            return this.BinaryExpr(expr);
        }

        public TrashObject VisitGroupingExpr(Expr.Grouping expr)
        {
            return this.GroupingExpr(expr);
        }
        
        public TrashObject VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public TrashObject VisitNewExpr(Expr.New expr)
        {
            return this.NewExpr(expr);
        }

        public TrashObject VisitUnaryExpr(Expr.Unary expr)
        {
            return this.UnaryExpr(expr);
        }

        public TrashObject VisitVariableExpr(Expr.Variable expr)
        {
            return this.VariableExpr(expr);
        }

        public TrashObject VisitAssignStmt(Stmt.Assign stmt)
        {
            return this.AssignStmt(stmt);
        }

        public TrashObject VisitBlockStmt(Stmt.Block stmt)
        {
            return this.BlockStmt(stmt);
        }

        public TrashObject VisitClassStmt(Stmt.Class stmt)
        {
            return this.ClassStmt(stmt);
        }

        public TrashObject VisitDefineStmt(Stmt.Define stmt)
        {
            return this.DefineStmt(stmt);
        }

        public TrashObject VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.InnerExpression);
            return null;
        }

        public TrashObject VisitIfStmt(Stmt.If stmt)
        {
            return this.IfStmt(stmt);
        }

        public TrashObject VisitMacroStmt(Stmt.Macro stmt)
        {
            return this.MacroStmt(stmt);
        }

        public TrashObject VisitMemberStmt(Stmt.Member stmt)
        {
            return this.MemberStmt(stmt);
        }

        public TrashObject VisitPrintStmt(Stmt.Print stmt)
        {
            return this.PrintStmt(stmt);
        }

        public TrashObject VisitRepeatStmt(Stmt.Repeat stmt)
        {
            return this.RepeatStmt(stmt);
        }

        public TrashObject VisitRequireStmt(Stmt.Require stmt)
        {
            return this.RequireStmt(stmt);
        }

        public TrashObject VisitReturnStmt(Stmt.Return stmt)
        {
            return this.ReturnStmt(stmt);
        }
    }

}