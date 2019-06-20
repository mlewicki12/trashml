
using System;
using TrashML.Main;
using TrashML.Objects;

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

        public static TrashObject ClassStmt(this Interpreter interpreter, Stmt.Class stmt)
        {
            var cls = new Class(stmt.Name);
            foreach (var def in stmt.Body.Statements)
            {
                if (def.Body is null)
                {
                    cls.Add(def.Name, interpreter.Evaluate(def.Initialiser));
                }
                else
                {
                    cls.Add(def.Name, new TrashObject(new Stmt.Macro(def.Name, def.Body)));
                }
            }
            
            interpreter.IntEnvironment.Define(stmt.Name, new TrashObject(cls));
            return null;
        }
    }
}