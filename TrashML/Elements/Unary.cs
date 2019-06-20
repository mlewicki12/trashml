
using TrashML.Main;
using TrashML.Objects;
using TrashML.Objects.Overrides;

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
        
        public static TrashObject UnaryExpr(this Interpreter interpreter, Expr.Unary expr)
        {
            var right = interpreter.Evaluate(expr.Right);
            return interpreter.RunOverride(expr.Operator, right);
        }
    }
}