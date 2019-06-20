
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
        
        public static void AddOverrides()
        {
            OverrideMap.AddOverride(Lexer.Token.TokenType.PLUS, 
                                    TrashObject.ObjectType.NUMBER, 
                                    TrashObject.ObjectType.NUMBER, 
                                    add);
            
            OverrideMap.AddOverride(Lexer.Token.TokenType.MINUS, 
                                    TrashObject.ObjectType.NUMBER, 
                                    TrashObject.ObjectType.NUMBER, 
                                    sub);
            
            OverrideMap.AddOverride(Lexer.Token.TokenType.MULTIPLY, 
                                    TrashObject.ObjectType.NUMBER, 
                                    TrashObject.ObjectType.NUMBER, 
                                    mul);
            
            OverrideMap.AddOverride(Lexer.Token.TokenType.DIVIDE, 
                                    TrashObject.ObjectType.NUMBER, 
                                    TrashObject.ObjectType.NUMBER, 
                                    div);
        }
        
    }
}