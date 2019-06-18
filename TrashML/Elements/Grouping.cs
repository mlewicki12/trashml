
namespace TrashML.Elements
{
    public static class GroupingExtension
    {
        public static Expr Grouping(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.LEFT_PAREN))
            {
                Expr expr = parser.Comparison();
                
                parser.Consume("Expected ')' after '('", Lexer.Token.TokenType.RIGHT_PAREN);
                return new Expr.Grouping(expr);
            }

            return parser.Primary();
        }

        public static object GroupingExpr(this Interpreter interpreter, Expr.Grouping expr)
        {
            return interpreter.Evaluate(expr.Expression);
        }
    }
}