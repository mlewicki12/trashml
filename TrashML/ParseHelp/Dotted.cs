
namespace TrashML.ParseHelp
{
    public static class DottedExtension
    {
        public static Expr Dotted(this Parser parser)
        {
            // this is probably not working yet, but the framework's there
            // so this should work at some point
            // like once I get classes working
            
            // actually I don't even have this used atm
            // so it doesn't work
            // but it will work someday
            Expr expr = parser.Primary();

            while (parser.Match(Lexer.Token.TokenType.DOT))
            {
                Lexer.Token op = parser.Previous();
                Expr right = parser.Unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }
    }
}