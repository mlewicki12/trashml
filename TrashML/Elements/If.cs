
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class IfExtension
    {
        public static Stmt If(this Parser parser)
        {
            var line = parser.Previous().Line;
            var cond = parser.Comparison();

            parser.Consume("Expected 'do' after 'if'", Lexer.Token.TokenType.DO);
            var blk = parser.Block();
            List<Stmt> blk2 = null;

            if (parser.Match(Lexer.Token.TokenType.ELSE))
            {
                blk2 = parser.Block();
            }

            return new Stmt.If(cond, new Stmt.Block(blk), (blk2 != null) ? new Stmt.Block(blk2) : null);
        }

        public static TrashObject IfStmt(this Interpreter interpreter, Stmt.If stmt)
        {
            var cond = interpreter.Evaluate(stmt.Condition).Access();

            if (cond is bool)
            {
                if ((bool) cond)
                {
                    interpreter.Execute(stmt.WhenTrue);
                }
                else if (stmt.WhenFalse != null)
                {
                    interpreter.Execute(stmt.WhenFalse);
                }

                return null;
            }

            throw new Interpreter.RuntimeError("If statement condition not a boolean value");
        }
    }
}