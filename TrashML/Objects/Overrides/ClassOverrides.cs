
using TrashML.Main;

namespace TrashML.Objects.Overrides
{
    public class ClassOverrides
    {
        private static TrashObject acc(TrashObject one, TrashObject two)
        {
            return one.Access(two.Access() as Lexer.Token) as TrashObject;
        }
        
        public static void AddOverrides()
        {
            OverrideMap.AddOverride(Lexer.Token.TokenType.DOT, TrashObject.ObjectType.CLASS, TrashObject.ObjectType.IDENTIFIER, acc);
        }
    }
}