
using System;
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    public class BooleanOverrides
    {
        private static TrashObject and(TrashObject one, TrashObject two)
        {
            return new TrashObject((bool) one.Access() && (bool) two.Access());
        }
        
        private static TrashObject or(TrashObject one, TrashObject two)
        {
            return new TrashObject((bool) one.Access() || (bool) two.Access());
        }

        private static TrashObject eq(TrashObject one, TrashObject two)
        {
            return new TrashObject((bool) one.Access() == (bool) two.Access());
        }
        
        private static TrashObject neq(TrashObject one, TrashObject two)
        {
            return new TrashObject((bool) one.Access() != (bool) two.Access());
        }

        private static void Add(Lexer.Token.TokenType op, Func<TrashObject, TrashObject, TrashObject> fnc)
        {
            OverrideMap.AddOverride(op, TrashObject.ObjectType.BOOL, TrashObject.ObjectType.BOOL, fnc);
        }

        public static void AddOverrides()
        {
            Add(Lexer.Token.TokenType.AND, and);
            Add(Lexer.Token.TokenType.OR, or);
            
            Add(Lexer.Token.TokenType.EQUAL_EQUAL, eq);
            Add(Lexer.Token.TokenType.BANG_EQUAL, neq);
        }
    }
}