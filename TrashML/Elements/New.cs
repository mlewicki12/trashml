

using System;
using TrashML.Main;

namespace TrashML.Elements
{
    public static class NewExtension
    {
        public static Expr New(this Parser parser)
        {
            Lexer.Token name = parser.Consume("Expected identifier after 'new'",Lexer.Token.TokenType.IDENTIFIER);
            return new Expr.New(name);
        }

        public static object NewExpr(this Interpreter interpreter, Expr.New value)
        {
            throw new NotImplementedException();
        }
    }
}