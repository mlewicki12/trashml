
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
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
            if (parser.Match(Lexer.Token.TokenType.DO)) blk = parser.Block();
            else throw new Parser.ParseError("Expected block after repeat statement", line);

            if (right != null)
            {
                return new Stmt.Repeat(left, right, blk);
            }

            return new Stmt.Repeat(left, blk);
        }

        public static TrashObject RepeatStmt(this Interpreter interpreter, Stmt.Repeat stmt)
        {
            // we've got ourselves a low-high expression bois
            if (stmt.LowValue != null)
            {
                var low = interpreter.Evaluate(stmt.LowValue).Access();
                var high = interpreter.Evaluate(stmt.HighValue).Access();
                if (low is int && high is int)
                {
                    for (int i = (int) low; i <= (int) high; ++i)
                    {
                        interpreter.Execute(stmt.Block);
                    }

                    return null;
                }

                throw new Interpreter.RuntimeError("Invalid values for repeat loop");
            }
            
            // otherwise this guy's got a condition
            var cond = interpreter.Evaluate(stmt.Condition).Access();
            if (cond is bool)
            {
                while ((bool) interpreter.Evaluate(stmt.Condition).Access())
                {
                    interpreter.Execute(stmt.Block);
                }
            }

            return null;
        }
    }
}