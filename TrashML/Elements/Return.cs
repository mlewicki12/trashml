
using System;

namespace TrashML.Elements
{
    public static class ReturnExpression
    {
        public static Stmt Return(this Parser parser)
        {
            var value = parser.Comparison();
            return new Stmt.Return(value);
        }

        public static string ReturnStmt(this Interpreter interpreter, Stmt.Return stmt)
        {
            throw new NotImplementedException();
        }
    }
}