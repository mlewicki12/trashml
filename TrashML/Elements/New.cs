

using System;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class NewExtension
    {
        public static Expr New(this Parser parser)
        {
            Lexer.Token name = parser.Consume("Expected identifier after 'new'",Lexer.Token.TokenType.IDENTIFIER);
            return new Expr.New(name);
        }

        public static TrashObject NewExpr(this Interpreter interpreter, Expr.New value)
        {
            return null;
        }
    }
}