
using System.Collections.Generic;

namespace TrashML.ParseHelp
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

            while (!parser.Check(Lexer.Token.TokenType.END) && !parser.IsAtEnd())
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

            parser.Consume("Expected 'end' after 'do'", Lexer.Token.TokenType.END);
            return statements;
        }   
    }
}