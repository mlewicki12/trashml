
using System;
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class DefineExtension
    {
        public static List<Stmt.Member> Define(this Parser parser)
        {
            // TODO: kill the newlines
            // I don't think I'm using newlines anywhere
            // so I should be able to remove them from the lexer
            while (parser.Match(Lexer.Token.TokenType.NEWLINE))
            {
            }

            List<Stmt.Member> statements = new List<Stmt.Member>();

            while (!parser.Check(Lexer.Token.TokenType.END) && !parser.IsAtEnd())
            {
                if (parser.Match(Lexer.Token.TokenType.MEMBER))
                {
                    var stmt = parser.Member();
                    if (stmt != null)
                    {
                        statements.Add(stmt);
                    }
                } 
                else if (parser.Match(Lexer.Token.TokenType.NEWLINE))
                {
                }
                else
                {
                    throw new Parser.ParseError("Expected 'member' definition or 'end' in class", parser.Previous().Line);
                }
            }

            parser.Consume("Expect 'end' after 'define'", Lexer.Token.TokenType.END);
            return statements;
        }

        public static TrashObject DefineStmt(this Interpreter interpreter, Stmt.Define stmt)
        {
            // this should also theoretically never be visited
            // but at this point it's just spaghetti, so I wouldn't be surprised if it was
            throw new NotImplementedException();
        }
    }
}