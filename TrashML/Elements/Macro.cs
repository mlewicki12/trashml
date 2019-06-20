
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class MacroExtension
    {
        public static Stmt Macro(this Parser parser)
        {
            parser.Consume("Expected identifier after macro definition",Lexer.Token.TokenType.IDENTIFIER);
            var id = parser.Previous();

            parser.Consume("Expected block after macro identifier", Lexer.Token.TokenType.DO);

            return new Stmt.Macro(id, new Stmt.Block(parser.Block()));
        }

        public static TrashObject MacroStmt(this Interpreter interpreter, Stmt.Macro stmt)
        {
            interpreter.IntEnvironment.Define(stmt.Name, new TrashObject(stmt));
            return null;
        }
    }
}