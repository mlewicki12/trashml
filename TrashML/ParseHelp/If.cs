
using System.Collections.Generic;

namespace TrashML.ParseHelp
{
    public static class IfExtension
    {
        public static Stmt If(this Parser parser)
        {
            var line = parser.Previous().Line;
            var cond = parser.Comparison();

            parser.Consume("Expected 'do' after 'if'", Lexer.Token.TokenType.DO);
            var blk = parser.Block();
            List<Stmt> blk2 = null;

            if (parser.Match(Lexer.Token.TokenType.ELSE))
            {
                line = parser.Previous().Line;
                parser.Consume("Expected 'do' after 'else'",Lexer.Token.TokenType.DO);
                blk2 = parser.Block();
            }

            return new Stmt.If(cond, new Stmt.Block(blk), (blk2 != null) ? new Stmt.Block(blk2) : null);
        }
    }
}