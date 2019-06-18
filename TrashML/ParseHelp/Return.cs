
namespace TrashML.ParseHelp
{
    public static class ReturnExpression
    {
        public static Stmt Return(this Parser parser)
        {
            var value = parser.Comparison();
            return new Stmt.Return(value);
        }
    }
}