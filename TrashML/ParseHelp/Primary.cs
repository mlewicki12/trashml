
namespace TrashML.ParseHelp
{
    public static class PrimaryExtension
    {
        public static Expr Primary(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.FALSE)) return new Expr.Literal(false);
            if (parser.Match(Lexer.Token.TokenType.TRUE)) return new Expr.Literal(true);

            if (parser.Match(Lexer.Token.TokenType.NUMBER)) return new Expr.Literal(int.Parse(parser.Previous().Literal));
            if (parser.Match(Lexer.Token.TokenType.IDENTIFIER)) return new Expr.Variable(parser.Previous());
            if (parser.Match( Lexer.Token.TokenType.STRING)) return new Expr.Literal(parser.Previous().Literal);

            throw new Parser.ParseError("Expecting expression", parser.Previous().Line);
        }
    }
}