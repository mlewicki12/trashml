
namespace TrashML.ParseHelp
{
    public static class RepeatExtension
    {
        public static Stmt Repeat(this Parser parser)
        {
            var line = parser.Previous().Line;
            
            Expr left = parser.Comparison();
            Expr right = null;

            // hey we've got a high low loop
            // also im exhausted
            if (parser.Match(Lexer.Token.TokenType.COLON, Lexer.Token.TokenType.TO))
            {
                right = parser.Comparison();
            }

            Stmt.Block blk = null;
            if (parser.Match(Lexer.Token.TokenType.DO)) blk = new Stmt.Block(parser.Block());
            else throw new Parser.ParseError("Expected block after repeat statement", line);

            if (right != null)
            {
                return new Stmt.Repeat(left, right, blk);
            }

            return new Stmt.Repeat(left, blk);
        }
    }
}