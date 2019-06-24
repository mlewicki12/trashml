
using System;
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class CommaExtension
    {
        public static Stmt.Comma Comma(this Parser parser)
        {
            var ids = new List<Lexer.Token>();
            
            do
            {
                // this should be generalised, since comma stmts are probably not only gonna be used after with
                ids.Add( parser.Consume("Expected identifier after 'with' statement", Lexer.Token.TokenType.IDENTIFIER));
            } while (parser.Match(Lexer.Token.TokenType.COMMA));

            return new Stmt.Comma(ids);
        }

        public static TrashObject CommaStmt(this Interpreter interpreter, Stmt.Comma stmt)
        {
            throw new NotImplementedException();   
        }
    }
}