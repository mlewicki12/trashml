
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class PrimaryExtension
    {
        public static Expr Primary(this Parser parser)
        {
            if (parser.Match(Lexer.Token.TokenType.FALSE)) return new Expr.Literal(new TrashObject(false));
            if (parser.Match(Lexer.Token.TokenType.TRUE)) return new Expr.Literal(new TrashObject(true));

            if (parser.Match(Lexer.Token.TokenType.NUMBER)) return new Expr.Literal(new TrashObject(int.Parse(parser.Previous().Literal)));
            if (parser.Match(Lexer.Token.TokenType.IDENTIFIER)) return parser.Variable();
            if (parser.Match(Lexer.Token.TokenType.WITH)) return parser.Arg();
            if (parser.Match( Lexer.Token.TokenType.STRING)) return new Expr.Literal(new TrashObject(parser.Previous().Literal));

            throw new Parser.ParseError("Expecting expression", parser.Previous().Line);
        }

        public static Expr Variable(this Parser parser)
        {
            var id = parser.Previous();
            
            Expr.Arg args = null;
            if (parser.Match(Lexer.Token.TokenType.WITH))
            {
                args = parser.Arg();
            }
            
            return new Expr.Variable(id, args);
        }
        
        public static TrashObject VariableExpr(this Interpreter interpreter, Expr.Variable expr)
        {
            var env = interpreter.IntEnvironment;
            while (env != null && !env.Contains(expr.Name))
            {
                // run through all the environments to check if we have the variable
                env = env.Enclosing;
            }

            if (env == null)
            {
                // this is an interesting issue
                // since the dot operator is treated as a binary operator, this happens when trying to access stuff
                // so for now, I might treat identifiers as strings and see if I can get classes working
                //throw new Interpreter.RuntimeError($"Trying to access non-existing variable {expr.Name.Literal}");
                
                return new TrashObject(expr.Name);
            }

            var value = env.Get(expr.Name).Access();

            if (value is Stmt.Macro) // if it's a macro, run it as a macro
            {
                // I'm lazy, so I'll just note it down here
                // the argument list on Macros doesn't actually matter
                // ie. since named arguments are passed in, the argument list is more of a documentation than anything
                
                var macro = value as Stmt.Macro;
                var newEnv = new Environment(macro.Name.Literal, interpreter.IntEnvironment);
                if (expr.Arguments != null)
                {
                    foreach (var assign in expr.Arguments.Values)
                    {
                        // you know
                        // I could probably have made this, so define takes an assign statement
                        // but again
                        // I'm lazy
                        // so I'll leave this here for when I stumble upon it again and decide to remake the function
                        newEnv.Define(assign.Name, interpreter.Evaluate(assign.Initialiser));
                    }    
                }
                
                return interpreter.ExecuteBlock(macro.Body.Statements, newEnv);
            }

            return env.Get(expr.Name);
        }
    }
}