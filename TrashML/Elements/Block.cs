
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class BlockExtension
    {
        public static List<Stmt> Block(this Parser parser)
        {
            // should get rid of pesky newlines
            while (parser.Match(Lexer.Token.TokenType.NEWLINE))
            {
            }

            List<Stmt> statements = new List<Stmt>();

            while (!parser.Check(Lexer.Token.TokenType.END, Lexer.Token.TokenType.ELSE) && !parser.IsAtEnd())
            {
                // we have a return statement yay
                if (parser.Match(Lexer.Token.TokenType.RETURN))
                {
                    var stmt = parser.Return();
                    if (stmt != null)
                    {
                        statements.Add(stmt);
                    }
                }
                else
                {
                    var stmt = parser.Statement();
                    if (stmt != null)
                    {
                        statements.Add(stmt);
                    }
                }
            }

            // I don't want it to eat the else
            if (parser.Check(Lexer.Token.TokenType.ELSE))
            {
                return statements;
            }
            
            parser.Consume("Expected 'end' after 'do'", Lexer.Token.TokenType.END);
            return statements;
        }

        public static TrashObject BlockStmt(this Interpreter interpreter, Stmt.Block stmt)
        {
            return interpreter.ExecuteBlock(stmt.Statements, new Environment(interpreter.IntEnvironment.Name + " Block", interpreter.IntEnvironment));
        }
    }
}