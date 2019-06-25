
using System.Collections.Generic;
using TrashML.Main;

namespace TrashML.Objects
{
    public class Class
    {
        public readonly string Name;
        private readonly Dictionary<string, TrashObject> _elements = new Dictionary<string, TrashObject>();
        private readonly List<Lexer.Token> _keys = new List<Lexer.Token>();

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
            if (!Exists(name))
            {
                _keys.Add(name);
                _elements.Add(name.Literal, body);
            }
            else _elements[name.Literal] = body;
            
            
            return this;
        }

        public List<Lexer.Token> Keys()
        {
            return _keys;
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
    }
}