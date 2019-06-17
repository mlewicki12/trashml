
namespace TrashML.ParseHelp
{
    public static class ComparisonExtension
    {
        public static Expr Comparison(this Parser parser)
        {
            Expr left = parser.Condition();

            if (parser.Match(Lexer.Token.TokenType.AND, Lexer.Token.TokenType.OR))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Condition();
                return new Expr.Binary(left, op, right);
            }

            return left;
        }   
    }
}