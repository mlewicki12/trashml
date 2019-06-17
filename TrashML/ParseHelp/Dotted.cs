
namespace TrashML.ParseHelp
{
    public static class DottedExtension
    {
        public static Stmt Dotted(this Parser parser)
        {
            // you weren't pretty enough
            // and I still need to fix you
            
            // is this pop punk
            
            // probably should just treat the dot as an operator
            var value = parser.Previous().Literal.Split('.');
            return new Stmt.Dotted(new Lexer.Token{Type = Lexer.Token.TokenType.IDENTIFIER, Literal = value[0]},
                new Lexer.Token{Type = Lexer.Token.TokenType.IDENTIFIER, Literal = value[1]});
        }
    }
}