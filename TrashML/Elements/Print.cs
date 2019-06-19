
using System;
using TrashML.Main;

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

        public static string PrintStmt(this Interpreter interpreter, Stmt.Print stmt)
        {
            object value = interpreter.Evaluate(stmt.Expression);
            Console.WriteLine(value);

            return "";
        }
    }
}