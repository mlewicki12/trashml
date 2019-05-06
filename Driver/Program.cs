
using System;

namespace TrashML.Driver
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Lexer lexer = new Lexer("let x = 5 + 5\nif x > 8 do\nprint x\nend else do\nprint 8\nend");
            var tokens = lexer.Scan();
            
            Parser parser = new Parser(tokens);
            var parsed = parser.Parse();
            
            Interpreter trashml = new Interpreter();
            trashml.Interpret(parsed);
        }
    }
}