
using System;
using System.Collections.Generic;
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    // ideally, I wanna have the ability to use overrides for both C# implementations and TrashML implementations
    public static class OverrideMap
    {
        // one could say this code is trash
        // get it?
        // do you get it?
        private static Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType>, Func<TrashObject, TrashObject>> _unary_overrides_c = 
            new Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType>, Func<TrashObject, TrashObject>>();
        private static Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>, Func<TrashObject, TrashObject, TrashObject>> _binary_overrides_c = 
            new Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>, Func<TrashObject, TrashObject, TrashObject>>();
        
        // I know, I'm trash
        private static Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType>, List<Stmt>> _unary_overrides_t = 
            new Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType>, List<Stmt>>();
        private static Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>, List<Stmt>> _binary_overrides_t = 
            new Dictionary<Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>, List<Stmt>>();

        public static void AddOverride(Lexer.Token.TokenType op, TrashObject.ObjectType one,
            TrashObject.ObjectType two, Func<TrashObject, TrashObject, TrashObject> func)
        {
            var key = new Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>(op, one, two);
            _binary_overrides_c.Add(key, func);
        }
        
        public static TrashObject RunOverride(this Interpreter interpreter, Lexer.Token op, TrashObject one, TrashObject two = null)
        {
            if (two != null)
            {
                var key = new Tuple<Lexer.Token.TokenType, TrashObject.ObjectType, TrashObject.ObjectType>(op.Type, one.GetType(), two.GetType());
                
                // TrashML overrides to be considered first 
                if (_binary_overrides_t.ContainsKey(key))
                {
                    // op1 and op2 are specifically defined variables for overrides
                    var env = new Environment(op.Literal, interpreter.IntEnvironment);
                    env.Define(new Lexer.Token {Literal = "op1"}, two);
                    env.Define(new Lexer.Token {Literal = "op2"}, two);
                    
                    return interpreter.ExecuteBlock(_binary_overrides_t[key], env);
                }

                if (_binary_overrides_c.ContainsKey(key))
                {
                    return _binary_overrides_c[key](one, two);
                }
                
                throw new Interpreter.RuntimeError($"Unable to find suitable override for {op.Literal} with types {one.GetType()}, {two.GetType()}");
            }

            var ukey = new Tuple<Lexer.Token.TokenType, TrashObject.ObjectType>(op.Type, one.GetType());

            if (_unary_overrides_t.ContainsKey(ukey))
            {
                var env = new Environment(op.Literal, interpreter.IntEnvironment);
                env.Define(new Lexer.Token {Literal = "op1"}, one);
                
                return interpreter.ExecuteBlock(_unary_overrides_t[ukey],
                    new Environment(op.Literal, interpreter.IntEnvironment));
            }

            if (_unary_overrides_c.ContainsKey(ukey))
            {
                return _unary_overrides_c[ukey](one);
            }
            
            throw new Interpreter.RuntimeError($"Unable to find suitable override for {op.Literal} with type {one.GetType()}");
        }
    }
}