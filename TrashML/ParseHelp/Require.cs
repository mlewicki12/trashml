
namespace TrashML.ParseHelp
{
    public static class RequireExtension
    {
        public static Stmt Require(this Parser parser)
        {
            return new Stmt.Require(parser.Primary());
        }
    }
}