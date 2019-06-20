
using System;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class ReturnExpression
    {
        public static Stmt Return(this Parser parser)
        {
            var value = parser.Comparison();
            return new Stmt.Return(value);
        }

        public static TrashObject ReturnStmt(this Interpreter interpreter, Stmt.Return stmt)
        {
            return interpreter.Evaluate(stmt.Value);
        }
    }
}