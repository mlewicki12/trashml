
namespace TrashML.ParseHelp
{
    public static class ConditionExtension
    {
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
    }
}