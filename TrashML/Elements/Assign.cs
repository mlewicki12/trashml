
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class AssignmentExtension
    {
        public static Stmt.Assign Assign(this Parser parser)
        {
            Lexer.Token name = parser.Consume("Expected identifier after 'let'",Lexer.Token.TokenType.IDENTIFIER);

            Expr initialiser = null;
            if (parser.Match(Lexer.Token.TokenType.EQUAL))
            {
                initialiser = parser.Comparison();
            }

            // this fucks up when doing with statements
            // shouldn't be needed, but that's a testing thing
//            parser.Consume("Expected new line after variable declaration", Lexer.Token.TokenType.NEWLINE,
//                Lexer.Token.TokenType.EOF);
            return new Stmt.Assign(name, initialiser);
        }

        public static TrashObject AssignStmt(this Interpreter interpreter, Stmt.Assign stmt)
        {
            var value = interpreter.Evaluate(stmt.Initialiser);

            interpreter.IntEnvironment.Define(stmt.Name, value);
            return null;
        }
    }
}