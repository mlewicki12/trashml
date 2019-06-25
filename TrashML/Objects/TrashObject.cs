
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
            CLASS, MACRO, IDENTIFIER
        }

        private readonly ObjectType _type;
        private readonly object _value;
        private readonly Environment _elements;

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
                _elements = new Environment($"{(val as Class).Name} Int", null);
                
                // set all elements to their default values
                foreach (var key in (val as Class).Keys())
                {
                    _elements.Define(key, (val as Class).Get(key));
                }
            } else if (val is Stmt.Macro)
            {
                _type = ObjectType.MACRO;
            } else if (val is Lexer.Token)
            {
                _type = ObjectType.IDENTIFIER;
            }

            _value = val;
        }

        // currently returns null if not a value
        // should probably make it throw an error?
        public object Access(Lexer.Token name = null)
        {
            if (_type == ObjectType.CLASS && name != null)
            {
                if (_elements.Contains(name))
                {
                    return _elements.Get(name);
                }
                
                throw new Interpreter.RuntimeError($"Trying to access non-existing element {name.Literal} of class {(_value as Class).Name}");
            }
            
            return _value;
        }

        public TrashObject Set(Lexer.Token name, TrashObject value)
        {
            _elements.Define(name, value);
            return this;
        }

        public Environment GetEnvironment()
        {
            return _elements;
        }

        public ObjectType GetType()
        {
            return _type;
        }
    }
}