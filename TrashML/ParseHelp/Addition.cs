
namespace TrashML.ParseHelp
{
    public static class AdditionExtension
    {
        public static Expr Addition(this Parser parser)
        {
            Expr expr = parser.Multiplication();

            while (parser.Match(Lexer.Token.TokenType.PLUS, Lexer.Token.TokenType.MINUS))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Multiplication();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
    }
}