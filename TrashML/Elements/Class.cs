
using System;
using TrashML.Main;

namespace TrashML.Elements
{
    public static class ClassExtension
    {
        public static Stmt Class(this Parser parser)
        {
            parser.Consume("Expected identifier after class definition", Lexer.Token.TokenType.IDENTIFIER);
            var id = parser.Previous();

            parser.Consume("Expected block after class identifier", Lexer.Token.TokenType.DEFINE);

            return new Stmt.Class(id, new Stmt.Define(parser.Define()));
        }

        public static string ClassStmt(this Interpreter interpreter, Stmt.Class stmt)
        {
            throw new NotImplementedException();
        }
    }
}