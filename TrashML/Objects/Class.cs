
using System.Collections.Generic;
using TrashML.Main;

namespace TrashML.Objects
{
    public class Class
    {
        public readonly string Name;
        private readonly Dictionary<string, TrashObject> _elements = new Dictionary<string, TrashObject>();

        public Class(Lexer.Token name)
        {
            Name = name.Literal;
        }

        public bool Exists(Lexer.Token name)
        {
            return _elements.ContainsKey(name.Literal);
        }

        public Class Add(Lexer.Token name, TrashObject body)
        {
            _elements.Add(name.Literal, body);
            return this;
        }

        public TrashObject Get(Lexer.Token name)
        {
            if (_elements.ContainsKey(name.Literal))
            {
                return _elements[name.Literal];
            }
            
            throw new Interpreter.RuntimeError($"Attempting to access non-existent element {name.Literal} of class {Name}");
        }

        public TrashObject Initialize()
        {
            // this might not work, depending on how the class is passed
            // but y'know, I'll test that at some point
            // TODO: redo stuff
            return new TrashObject(this);
        }

        // determine an override based on types
        // this would be a great idea to introduce polymorphism, but I'm lazy
        public TrashObject BinaryOperation(Lexer.Token op, TrashObject other)
        {
            return null;
        }

        public TrashObject UnaryOperator(Lexer.Token op)
        {
            return null;
        }
    }
}