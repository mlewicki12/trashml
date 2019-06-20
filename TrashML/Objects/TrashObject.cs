
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects.Overrides;

namespace TrashML.Objects
{
    public class TrashObject
    {

        public enum ObjectType
        {
            NUMBER, STRING, BOOL,
            CLASS, MACRO
        }

        private readonly ObjectType _type;
        private readonly object _value;

        public TrashObject(object val)
        {
            if (val is int || val is double)
            {
                _type = ObjectType.NUMBER;
            } else if (val is string)
            {
                _type = ObjectType.STRING;
            } else if (val is bool)
            {
                _type = ObjectType.BOOL;
            } else if (val is Class)
            {
                _type = ObjectType.CLASS;
            } else if (val is Stmt.Macro)
            {
                _type = ObjectType.MACRO;
            }

            _value = val;
        }

        public object Access(Lexer.Token name = null)
        {
            if (_type == ObjectType.CLASS && name != null)
            {
                return ((Class) _value).Get(name);
            }
            
            return _value;
        }

        public ObjectType GetType()
        {
            return _type;
        }
    }
}