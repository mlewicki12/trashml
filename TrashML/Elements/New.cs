

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
            
            Expr.Arg args = null;
            if (parser.Match(Lexer.Token.TokenType.WITH))
            {
                args = parser.Arg();
            }
            
            return new Expr.New(name, args);
        }

        public static TrashObject NewExpr(this Interpreter interpreter, Expr.New value)
        {
            var cls = interpreter.IntEnvironment.GetClass(value.Identifier);
            var ret = cls.Initialize();

            // this is how it should work, but atm, everything's broken
            //var env = new Environment($"{cls.Name} cons", ret.GetEnvironment());
            var env = ret.GetEnvironment();
            if (!(value.Arguments is null))
            {
                foreach (var key in value.Arguments.Values)
                {
                    env.Define(key.Name, interpreter.Evaluate(key.Initialiser));
                }
            }
            
            // this kinda works
            // but since the assign statement expects an identifier, it doesn't
            // this whole codebase is begging to be shot in the face, so a rewrite would be worth it
            env.Define(new Lexer.Token {Literal = "this"}, ret);

            // I've worked myself into a hole with class environments
            // there is two here, the one being initialized on the class, and the one on the macro
            
            // check for the constructor
            var name = new Lexer.Token {Literal = "con"};
            if (cls.Exists(name))
            {
                // right now, this works on the internal macro environment
                // so any values changed will not be affected
                var con = (ret.Access(name) as TrashObject).Access() as Stmt.Macro;
                interpreter.ExecuteBlock(con.Body.Statements, env);
            }

            return ret;
        }
    }
}