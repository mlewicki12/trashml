
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class StatementExtension
    {
        public static Stmt Statement(this Parser parser)
        {
            try
            {
                if (parser.Match(Lexer.Token.TokenType.CLASS)) return parser.Class();
                if (parser.Match(Lexer.Token.TokenType.DO)) return parser.Block();
                if (parser.Match(Lexer.Token.TokenType.LET)) return parser.Assign();
                if (parser.Match(Lexer.Token.TokenType.REPEAT)) return parser.Repeat();
                if (parser.Match(Lexer.Token.TokenType.REQUIRE)) return parser.Require();
                if (parser.Match(Lexer.Token.TokenType.IF)) return parser.If();
                if (parser.Match(Lexer.Token.TokenType.MACRO)) return parser.Macro();
                if (parser.Match(Lexer.Token.TokenType.PRINT)) return parser.Print();
                if (parser.Match(Lexer.Token.TokenType.NEWLINE)) return null; // kill any newlines

                return new Stmt.Expression(parser.Comparison());
            }
            catch (Parser.ParseError e)
            {
                parser.Errors.Add(e);
                parser.Synchronize();
            }

            return null; // fuck me, it's an error
        }
    }
}