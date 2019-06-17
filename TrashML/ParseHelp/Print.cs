
namespace TrashML.ParseHelp
{
    public static class PrintExtension
    {
        public static Stmt Print(this Parser parser)
        {
            var expr = parser.Comparison();

            parser.Consume("Expected new line after variable declaration", Lexer.Token.TokenType.NEWLINE,
                Lexer.Token.TokenType.EOF);
            
            return new Stmt.Print(expr);
        } 
    }
}