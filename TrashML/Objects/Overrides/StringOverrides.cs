
using System;
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    public class StringOverrides
    {
        private static TrashObject add(TrashObject one, TrashObject two)
        {
            return new TrashObject((string) one.Access() + (string) two.Access());
        }
        
        private static TrashObject eq(TrashObject one, TrashObject two)
        {
            return new TrashObject((string) one.Access() == (string) two.Access());
        }
        
        private static TrashObject neq(TrashObject one, TrashObject two)
        {
            return new TrashObject((string) one.Access() != (string) two.Access());
        }

        private static TrashObject lt(TrashObject one, TrashObject two)
        {
            var res = Compare(one, two);
            return new TrashObject(res == 1);
        }

        private static TrashObject le(TrashObject one, TrashObject two)
        {
            var res = Compare(one, two);
            return new TrashObject(res >= 0);
        }
        
        private static TrashObject gt(TrashObject one, TrashObject two)
        {
            var res = Compare(one, two);
            return new TrashObject(res == -1);
        }

        private static TrashObject ge(TrashObject one, TrashObject two)
        {
            var res = Compare(one, two);
            return new TrashObject(res <= 0);
        }
        
        /*
         * returns 1  if one is greater than two (greater meaning earlier in the alphabet, so a > b)
         * returns 0  if equal
         * returns -1 if one is lesser than two 
         */
        private static int Compare(TrashObject on, TrashObject tw)
        {
            var one = (string) on.Access();
            var two = (string) tw.Access();
            
            if (one.Length < two.Length)
            {
                return -1;
            } else if (one.Length > two.Length)
            {
                return 1;
            }

            for (var i = 0; i < one.Length; ++i)
            {
                if (one[i] < two[i])
                {
                    return 1;
                }

                if (one[i] > two[i])
                {
                    return -1;
                }
            }

            return 0;

        }
        
        private static void Add(Lexer.Token.TokenType op, Func<TrashObject, TrashObject, TrashObject> fnc)
        {
            OverrideMap.AddOverride(op, TrashObject.ObjectType.STRING, TrashObject.ObjectType.STRING, fnc);
        }

        public static void AddOverrides()
        {
            Add(Lexer.Token.TokenType.PLUS, add);
            
            Add(Lexer.Token.TokenType.EQUAL_EQUAL, eq);
            Add(Lexer.Token.TokenType.BANG_EQUAL, neq);
            Add(Lexer.Token.TokenType.LESS, lt);
            Add(Lexer.Token.TokenType.LESS_EQUAL, le);
            Add(Lexer.Token.TokenType.GREATER, gt);
            Add(Lexer.Token.TokenType.GREATER_EQUAL, ge);
        }
    }
}