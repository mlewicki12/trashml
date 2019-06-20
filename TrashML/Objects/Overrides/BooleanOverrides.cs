
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    public class BooleanOverrides
    {
        private static TrashObject and(TrashObject one, TrashObject two)
        {
            var oneVal = one.Access();
            var twoVal = two.Access();
            return new TrashObject((bool) oneVal && (bool) twoVal);
        }
        
        private static TrashObject or(TrashObject one, TrashObject two)
        {
            var oneVal = one.Access();
            var twoVal = two.Access();
            return new TrashObject((bool) oneVal || (bool) twoVal);
        }

        public static void AddOverrides()
        {
            OverrideMap.AddOverride(Lexer.Token.TokenType.AND,
                                        TrashObject.ObjectType.BOOL,
                                        TrashObject.ObjectType.BOOL,
                                        and);
            
            OverrideMap.AddOverride(Lexer.Token.TokenType.OR,
                                        TrashObject.ObjectType.BOOL,
                                        TrashObject.ObjectType.BOOL,
                                        or);
        }
    }
}