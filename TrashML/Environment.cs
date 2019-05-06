
using System;
using System.Collections.Generic;

namespace TrashML
{
    public class Environment
    {
        public Environment Enclosing;
        private Dictionary<string, Object> values = new Dictionary<string, Object>();

        public Environment() : this(null)
        {
        }

        public Environment(Environment enclosing)
        {
            this.Enclosing = enclosing;
        }

        public object Get(Lexer.Token name)
        {
            if (values.ContainsKey(name.Literal))
            {
                return values[name.Literal];
            }

            if (Enclosing != null) return Enclosing.Get(name);

            throw new Interpreter.RuntimeError("Attempting to access undefined variable " + name.Literal);
        }

        public bool Contains(Lexer.Token name)
        {
            return values.ContainsKey(name.Literal);
        }

        public void Define(Lexer.Token name, Object value)
        {
            if (!values.ContainsKey(name.Literal))
            {
                var encl = Enclosing;
                while (encl != null)
                {
                    if (encl.Contains(name))
                    {
                        encl.Define(name, value);
                        break;
                    }
                }

                values.Add(name.Literal, value);
            }

            values[name.Literal] = value;
        }
    }
}

