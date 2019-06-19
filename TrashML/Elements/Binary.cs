
using TrashML.Main;

namespace TrashML.Elements
{
    public static class BinaryExtension
    {
        public static Expr Comparison(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.NEW))
            {
                return parser.New();
            }
            
            Expr left = parser.Condition();

            if (parser.Match(Lexer.Token.TokenType.AND, Lexer.Token.TokenType.OR))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Condition();
                return new Expr.Binary(left, op, right);
            }

            return left;
        }   
        
        public static Expr Condition(this Parser parser)
        {
            Expr left = parser.Addition();

            while (parser.Match(Lexer.Token.TokenType.EQUAL_EQUAL, Lexer.Token.TokenType.BANG_EQUAL,
                Lexer.Token.TokenType.LESS, Lexer.Token.TokenType.LESS_EQUAL,
                Lexer.Token.TokenType.GREATER, Lexer.Token.TokenType.GREATER_EQUAL))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Addition();
                left = new Expr.Binary(left, op, right);
            }

            return left;
        }
        
        public static Expr Addition(this Parser parser)
        {
            Expr left = parser.Multiplication();

            while (parser.Match(Lexer.Token.TokenType.PLUS, Lexer.Token.TokenType.MINUS))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Multiplication();
                left = new Expr.Binary(left, op, right);
            }

            return left;
        }
        
        public static Expr Multiplication(this Parser parser)
        {
            Expr expr = parser.Unary();

            while (parser.Match(Lexer.Token.TokenType.MULTIPLY, Lexer.Token.TokenType.DIVIDE))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        public static Expr Dotted(this Parser parser)
        {
            Expr expr = parser.Grouping();

            while (parser.Match(Lexer.Token.TokenType.DOT))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Grouping();
                expr = new Expr.Binary(expr, op, right);
            }
            
            return expr;
        }
        
        public static object BinaryExpr(this Interpreter interpreter, Expr.Binary expr)
        {
            var left = interpreter.Evaluate(expr.Left);
            var right = interpreter.Evaluate(expr.Right);

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

            if (left is bool && right is bool)
            {
                switch (expr.Operator.Type)
                {
                    case Lexer.Token.TokenType.AND:
                        return (bool) left && (bool) right;
                    
                    case Lexer.Token.TokenType.OR:
                        return (bool) left || (bool) right;
                }
            }

            throw new Interpreter.RuntimeError($"Unknown operator '{expr.Operator.Literal}'");
        }
    }
}