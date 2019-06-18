
namespace TrashML.Elements
{
    public static class PrimaryExtension
    {
        public static Expr Primary(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.FALSE)) return new Expr.Literal(false);
            if (parser.Match(Lexer.Token.TokenType.TRUE)) return new Expr.Literal(true);

            if (parser.Match(Lexer.Token.TokenType.NUMBER)) return new Expr.Literal(int.Parse(parser.Previous().Literal));
            if (parser.Match(Lexer.Token.TokenType.IDENTIFIER)) return new Expr.Variable(parser.Previous());
            if (parser.Match( Lexer.Token.TokenType.STRING)) return new Expr.Literal(parser.Previous().Literal);

            throw new Parser.ParseError("Expecting expression", parser.Previous().Line);
        }
        
        public static object VariableExpr(this Interpreter interpreter, Expr.Variable expr)
        {
            var env = interpreter.IntEnvironment;
            while (env != null && !env.Contains(expr.Name))
            {
                // run through all the environments to check if we have the variable
                env = env.Enclosing;
            }

            if (env == null)
            {
                throw new Interpreter.RuntimeError($"Trying to access non-existing variable {expr.Name.Literal}");
            }

            var value = env.Get(expr.Name);

            if (value is Stmt.Macro) // if it's a macro, run it as a macro
            {
                var macro = value as Stmt.Macro;
                interpreter.ExecuteBlock(macro.Body.Statements, new Environment(macro.Name.Literal, interpreter.IntEnvironment));
                return true;
            }

            return env.Get(expr.Name);
        }
    }
}