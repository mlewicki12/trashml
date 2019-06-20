
using System;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class PrintExtension
    {
        public static Stmt Print(this Parser parser)
        {
            var expr = parser.Comparison();

            parser.Consume("Expected new line after print statement", Lexer.Token.TokenType.NEWLINE,
                Lexer.Token.TokenType.EOF);
            
            return new Stmt.Print(expr);
        }

        public static TrashObject PrintStmt(this Interpreter interpreter, Stmt.Print stmt)
        {
            var value = interpreter.Evaluate(stmt.Expression);
            Console.WriteLine(value.Access());

            return null;
        }
    }
}