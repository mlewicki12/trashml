
using System;
using System.Collections.Generic;

namespace TrashML
{
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
                    execute(stmt);
                }
            }
            catch (RuntimeError e)
            {
                Errors.Add(e);
            }
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            var left = evaluate(expr.Left);
            var right = evaluate(expr.Right);

            if (left is int && right is int)
            {
                switch (expr.Operator.Type)
                {
                    case Lexer.Token.TokenType.PLUS:
                        return (int) left + (int) right;

                    case Lexer.Token.TokenType.MINUS:
                        return (int) left - (int) right;

                    case Lexer.Token.TokenType.MULTIPLY:
                        return (int) left * (int) right;

                    case Lexer.Token.TokenType.DIVIDE:
                        return (int) left / (int) right;

                    case Lexer.Token.TokenType.EQUAL:
                        return (int) left == (int) right;

                    case Lexer.Token.TokenType.BANG_EQUAL:
                        return (int) left != (int) right;

                    case Lexer.Token.TokenType.LESS:
                        return (int) left < (int) right;

                    case Lexer.Token.TokenType.LESS_EQUAL:
                        return (int) left <= (int) right;

                    case Lexer.Token.TokenType.GREATER:
                        return (int) left > (int) right;

                    case Lexer.Token.TokenType.GREATER_EQUAL:
                        return (int) left >= (int) right;
                }
            }

            throw new RuntimeError($"Unknown operator '{expr.Operator.Literal}'");
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return evaluate(expr.Expression);
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            var right = evaluate(expr.Right);

            if (right is int)
            {
                if (expr.Operator.Type == Lexer.Token.TokenType.MINUS)
                {
                    return -1 * (int) right;
                }

            }
            else if (right is bool)
            {
                if (expr.Operator.Type == Lexer.Token.TokenType.BANG)
                {
                    return !((bool) right);
                }
            }

            throw new RuntimeError($"Wrong type give for {expr.Operator.Literal} operator");
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            var env = IntEnvironment;
            while (env != null && !env.Contains(expr.Name))
            {
                // run through all the environments to check if we have the variable
                env = env.Enclosing;
            }

            if (env == null)
            {
                throw new RuntimeError($"Trying to access non-existing variable {expr.Name.Literal}");
            }

            var value = env.Get(expr.Name);

            if (value is Stmt.Macro) // if it's a macro, run it as a macro
            {
                var macro = value as Stmt.Macro;
                executeBlock(macro.Body.Statements, new Environment(macro.Name.Literal, IntEnvironment));
                return true;
            }

            return env.Get(expr.Name);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public string VisitBlockStmt(Stmt.Block stmt)
        {
            executeBlock(stmt.Statements, new Environment(IntEnvironment.Name + " Block", IntEnvironment));
            return "";
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.InnerExpression);
            return "";
        }

        public string VisitLetStmt(Stmt.Assign stmt)
        {
            object value = evaluate(stmt.Initialiser);

            IntEnvironment.Define(stmt.Name, value);
            return "";
        }

        public string VisitPrintStmt(Stmt.Print stmt)
        {
            object value = evaluate(stmt.Expression);
            Console.WriteLine(value);

            return "";
        }

        public string VisitRepeatStmt(Stmt.Repeat stmt)
        {
            // we've got ourselves a low-high expression bois
            if (stmt.LowValue != null)
            {
                var low = evaluate(stmt.LowValue);
                var high = evaluate(stmt.HighValue);
                if (low is int && high is int)
                {
                    for (int i = (int) low; i <= (int) high; ++i)
                    {
                        execute(stmt.Block);
                    }

                    return "";
                }

                throw new RuntimeError("Invalid values for repeat loop");
            }
            
            // otherwise this guy's got a condition
            var cond = evaluate(stmt.Condition);
            if (cond is bool)
            {
                while ((bool) evaluate(stmt.Condition))
                {
                    execute(stmt.Block);
                }
            }

            return "";
        }

        public string VisitDottedStmt(Stmt.Dotted stmt)
        {
            // you were used for unity, weren't you
            throw new NotImplementedException();
        }

        public string VisitIfStmt(Stmt.If stmt)
        {
            var cond = evaluate(stmt.Condition);

            if (cond is bool)
            {
                if ((bool) cond)
                {
                    execute(stmt.WhenTrue);
                }
                else if (stmt.WhenFalse != null)
                {
                    execute(stmt.WhenFalse);
                }

                return "";
            }

            throw new RuntimeError("If statement condition not a boolean value");
        }

        public string VisitMacroStmt(Stmt.Macro stmt)
        {
            IntEnvironment.Define(stmt.Name, stmt);
            return "";
        }

        public bool Error()
        {
            return Errors.Count != 0;
        }

        object evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        void execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        void executeBlock(List<Stmt> statements, Environment env)
        {
            Environment previous = IntEnvironment;

            try
            {
                IntEnvironment = env;

                foreach (var statement in statements)
                {
                    execute(statement);
                }
            }
            finally
            {
                IntEnvironment = previous;
            }
        }
    }

}