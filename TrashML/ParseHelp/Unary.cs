
namespace TrashML.ParseHelp
{
    public static class UnaryExtension
    {
        public static Expr Unary(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.LEFT_PAREN)) return parser.Grouping();
            if (parser.Match(Lexer.Token.TokenType.BANG, Lexer.Token.TokenType.MINUS))
                return new Expr.Unary(parser.Previous(), parser.Unary());

            return parser.Primary();
        }
    }
}