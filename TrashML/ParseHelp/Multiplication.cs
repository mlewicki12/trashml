
namespace TrashML.ParseHelp
{
    public static class MultiplicationExtension
    {
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
    }
}