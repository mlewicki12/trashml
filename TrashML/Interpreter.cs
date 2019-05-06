
using System;
using System.Collections.Generic;

namespace TrashML
{
    public class Interpreter : Expr.Visitor<System.Object>,
        Stmt.Visitor<string> // this string doesn't actually matter, but we can't use Void in C#
    {
        public class RuntimeError : Exception
        {
            public RuntimeError(string msg) : base(msg)
            {
            }
        }

        public Environment IntEnvironment = new Environment();
        public Environment Macros = new Environment();

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
                // do something
            }
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = evaluate(expr.Left);
            object right = evaluate(expr.Right);

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

            throw new RuntimeError($"Unknown operator '{expr.Operator.Literal}'");
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return evaluate(expr.Expression);
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = evaluate(expr.Right);

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
            if (this.Macros.Contains(expr.Name))
            {
                executeBlock((this.Macros.Get(expr.Name) as Stmt.Macro).Body.Statements,
                    new Environment(IntEnvironment));
                return true;
            }

            return IntEnvironment.Get(expr.Name);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public string VisitBlockStmt(Stmt.Block stmt)
        {
            executeBlock(stmt.Statements, new Environment(IntEnvironment));
            return "";
        }

        public string VisitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.InnerExpression);
            return "";
        }

        public string VisitAssignStmt(Stmt.Assign stmt)
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
            else // this guy's got a condition
            {
                var cond = evaluate(stmt.Condition);
                if (cond is bool)
                {
                    while ((bool) evaluate(stmt.Condition))
                    {
                        execute(stmt.Block);
                    }
                }
            }

            return "";
        }

        string intToDir(int num)
        {
            switch (num)
            {
                case 0:
                    return "up";

                case 1:
                    return "right";

                case 2:
                    return "down";

                case 3:
                    return "left";
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
            Macros.Define(stmt.Name, stmt);
            return "";
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
            Environment previous = this.IntEnvironment;

            try
            {
                this.IntEnvironment = env;

                foreach (var statement in statements)
                {
                    execute(statement);
                }
            }
            finally
            {
                this.IntEnvironment = previous;
            }
        }
    }

}