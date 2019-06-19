
using System;
using System.Collections.Generic;
using TrashML.Main;

namespace TrashML
{
    public class Environment
    {
        public readonly Environment Enclosing;
        public readonly string Name;
        
        private readonly Dictionary<string, object> _macros = new Dictionary<string, object>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public Environment(string name, Environment enclosing)
        {
            Enclosing = enclosing;
            Name = name;
        }

        public object Get(Lexer.Token name)
        {
            if (_values.ContainsKey(name.Literal))
            {
                return _values[name.Literal];
            }

            if (_macros.ContainsKey(name.Literal))
            {
                return _macros[name.Literal];
            }

            if (Enclosing != null) return Enclosing.Get(name);

            throw new Interpreter.RuntimeError("Attempting to access undefined variable " + name.Literal);
        }

        public bool Contains(Lexer.Token name)
        {
            return _values.ContainsKey(name.Literal) || _macros.ContainsKey(name.Literal);
        }

        public void Define(Lexer.Token name, object value)
        {
            if (value is Stmt.Macro)
            {
                if (!_macros.ContainsKey(name.Literal))
                {
                    _macros.Add(name.Literal, value);
                }
                else _values[name.Literal] = value;
            } else if (!_values.ContainsKey(name.Literal))
            {
                _values.Add(name.Literal, value);
            } else _values[name.Literal] = value;
        }
    }
}

