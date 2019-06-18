
namespace TrashML.ParseHelp
{
    public static class AdditionExtension
    {
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
    }
}