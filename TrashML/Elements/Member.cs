
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
            Stmt.Block block = null;
            if (parser.Match(Lexer.Token.TokenType.EQUAL))
            {
                initialiser = parser.Comparison();
            } else if (parser.Match(Lexer.Token.TokenType.DO))
            {
                block = parser.Block();
            }

            // if there is a block, it's a method
            if (block != null)
            {
                return new Stmt.Member(name, block);
            }
            
            return new Stmt.Member(name, initialiser);
        }

        public static TrashObject MemberStmt(this Interpreter interpreter, Stmt.Member stmt)
        {
            throw new NotImplementedException();
        }
    }
}