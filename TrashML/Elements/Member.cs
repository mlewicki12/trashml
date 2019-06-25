
using System;
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class MemberExtension
    {
        public static Stmt.Member Member(this Parser parser)
        {
            Lexer.Token name = parser.Consume("Expected identifier after 'member'", Lexer.Token.TokenType.IDENTIFIER);

            Expr initialiser = null;
            if (parser.Match(Lexer.Token.TokenType.EQUAL))
            {
                initialiser = parser.Comparison();
                return new Stmt.Member(name, initialiser);
            }

            Stmt.Comma args = null;
            if (parser.Match(Lexer.Token.TokenType.WITH))
            {
                args = parser.Comma();
            }
            
            Stmt.Block block = null;
            if (parser.Match(Lexer.Token.TokenType.DO))
            {
                block = parser.Block();
            }
            
            return new Stmt.Member(name, block, args);
        }

        public static TrashObject MemberStmt(this Interpreter interpreter, Stmt.Member stmt)
        {
            // this is NOT where we add the member to the class
            // this should theoretically never be visited
            throw new NotImplementedException();
        }
    }
}