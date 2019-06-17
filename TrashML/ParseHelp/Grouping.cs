
namespace TrashML.ParseHelp
{
    public static class GroupingExtension
    {
        public static Expr Grouping(this Parser parser)
        {
            Expr expr = parser.Comparison();

            parser.Consume("Expected ')' after '('", Lexer.Token.TokenType.RIGHT_PAREN);
            return new Expr.Grouping(expr);
        }
    }
}