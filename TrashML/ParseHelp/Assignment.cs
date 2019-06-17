
namespace TrashML.ParseHelp
{
    public static class AssignmentExtension
    {
        public static Stmt Assignment(this Parser parser)
        {
            Lexer.Token name = parser.Consume("Expected identifier after 'let'",Lexer.Token.TokenType.IDENTIFIER);

            Expr initialiser = null;
            if (parser.Match(Lexer.Token.TokenType.EQUAL))
            {
                initialiser = parser.Comparison();
            }

            parser.Consume("Expected new line after variable declaration", Lexer.Token.TokenType.NEWLINE,
                Lexer.Token.TokenType.EOF);
            return new Stmt.Assign(name, initialiser);
        }
    }
}