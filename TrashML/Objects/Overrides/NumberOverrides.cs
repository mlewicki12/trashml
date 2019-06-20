
using System;
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    public class NumberOverrides
    {
        private static TrashObject add(TrashObject one, TrashObject two)
        {
            var oneVal = one.Access();
            var twoVal = two.Access();
            return new TrashObject((int)oneVal + (int)twoVal);
        }
        
        private static TrashObject sub(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() - (int) two.Access());
        }
        
        private static TrashObject mul(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() * (int) two.Access());
        }
        
        private static TrashObject div(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() / (int) two.Access());
        }

        private static TrashObject eq(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() == (int) two.Access());
        }

        private static TrashObject neq(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() != (int) two.Access());
        }

        private static TrashObject lt(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() < (int) two.Access());
        }

        private static TrashObject le(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() <= (int) two.Access());
        }

        private static TrashObject gt(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() > (int) two.Access());
        }

        private static TrashObject ge(TrashObject one, TrashObject two)
        {
            return new TrashObject((int) one.Access() >= (int) two.Access());
        }

        private static void Add(Lexer.Token.TokenType op, Func<TrashObject, TrashObject, TrashObject> fnc)
        {
            OverrideMap.AddOverride(op, TrashObject.ObjectType.NUMBER, TrashObject.ObjectType.NUMBER, fnc);
        }
        
        public static void AddOverrides()
        {
            Add(Lexer.Token.TokenType.PLUS, add);
            Add(Lexer.Token.TokenType.MINUS, sub);
            Add(Lexer.Token.TokenType.MULTIPLY, mul);
            Add(Lexer.Token.TokenType.DIVIDE, div);
            
            Add(Lexer.Token.TokenType.EQUAL_EQUAL, eq);
            Add(Lexer.Token.TokenType.BANG_EQUAL, neq);
            Add(Lexer.Token.TokenType.LESS, lt);
            Add(Lexer.Token.TokenType.LESS_EQUAL, le);
            Add(Lexer.Token.TokenType.GREATER, gt);
            Add(Lexer.Token.TokenType.GREATER_EQUAL, ge);
        }
        
    }
}