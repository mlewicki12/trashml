
using System;
using System.Collections.Generic;

namespace TrashML
{
    public class Environment
    {
        public Environment Enclosing;
        public string Name;
        
        private Dictionary<string, object> macros = new Dictionary<string, object>();
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public Environment(string name, Environment enclosing)
        {
            Enclosing = enclosing;
            Name = name;
        }

        public object Get(Lexer.Token name)
        {
            if (values.ContainsKey(name.Literal))
            {
                return values[name.Literal];
            }

            if (macros.ContainsKey(name.Literal))
            {
                return macros[name.Literal];
            }

            if (Enclosing != null) return Enclosing.Get(name);

            throw new Interpreter.RuntimeError("Attempting to access undefined variable " + name.Literal);
        }

        public bool Contains(Lexer.Token name)
        {
            return values.ContainsKey(name.Literal) || macros.ContainsKey(name.Literal);
        }

        public void Define(Lexer.Token name, object value)
        {
            if (value is Stmt.Macro)
            {
                if (!macros.ContainsKey(name.Literal))
                {
                    macros.Add(name.Literal, value);
                }
                else values[name.Literal] = value;
            } else if (!values.ContainsKey(name.Literal))
            {
                values.Add(name.Literal, value);
            } else values[name.Literal] = value;
        }
    }
}

