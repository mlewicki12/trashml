
using System.Collections.Generic;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML
{
    public class Environment
    {
        public readonly Environment Enclosing;
        public readonly string Name;
        
        private readonly Dictionary<string, TrashObject> _values = new Dictionary<string, TrashObject>();

        public Environment(string name, Environment enclosing)
        {
            Enclosing = enclosing;
            Name = name;
        }

        public TrashObject Get(Lexer.Token name)
        {
            if (_values.ContainsKey(name.Literal))
            {
                return _values[name.Literal];
            }

            if (Enclosing != null) return Enclosing.Get(name);

            throw new Interpreter.RuntimeError("Attempting to access undefined variable " + name.Literal);
        }

        public bool Contains(Lexer.Token name)
        {
            return _values.ContainsKey(name.Literal);
        }

        public void Define(Lexer.Token name, TrashObject value)
        {
            if (!_values.ContainsKey(name.Literal))
            {
                _values.Add(name.Literal, value);
            }
            else _values[name.Literal] = value;
        }
    }
}

