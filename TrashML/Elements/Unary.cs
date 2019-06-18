
namespace TrashML.Elements
{
    public static class UnaryExtension
    {
        public static Expr Unary(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.BANG, Lexer.Token.TokenType.MINUS))
                return new Expr.Unary(parser.Previous(), parser.Unary());

            return parser.Dotted();
        }
        
        public static object UnaryExpr(this Interpreter interpreter, Expr.Unary expr)
        {
            var right = interpreter.Evaluate(expr.Right);

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

            throw new Interpreter.RuntimeError($"Wrong type give for {expr.Operator.Literal} operator");
        }
    }
}