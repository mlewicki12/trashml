
namespace TrashML.Elements
{
    public static class AssignmentExtension
    {
        public static Stmt Assign(this Parser parser)
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

        public static string AssignStmt(this Interpreter interpreter, Stmt.Assign stmt)
        {
            object value = interpreter.Evaluate(stmt.Initialiser);

            interpreter.IntEnvironment.Define(stmt.Name, value);
            return "";
        }
    }
}