
using System;
using System.Collections.Generic;
using System.Linq;
using TrashML.Elements;

namespace TrashML.Main
{
    public class Parser
    {
        public class ParseError : Exception
        {
            public readonly int Line;
            
            public ParseError(string msg, int line) : base(msg)
            {
                Line = line + 1;
            }

            public override string ToString()
            {
                return $"TrashML Parse Error, line {Line}: {Message}";
            }
        }

        public List<Lexer.Token> Tokens;
        public List<ParseError> Errors;

        private int _current;

        public Parser(List<Lexer.Token> tokens)
        {
            _current = 0;
            Tokens = tokens;
            Errors = new List<ParseError>();
        }

        public List<Stmt> Parse(List<Lexer.Token> tokens)
        {
            Tokens = tokens;
            return Parse();
        }

        public List<Stmt> Parse()
        {
            Errors = new List<ParseError>();
            
            var statements = new List<Stmt>();

            while (!IsAtEnd())
            {
                Stmt stmt = this.Statement();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
            }

            return statements;
        }
        
        public bool Error()
        {
            return Errors.Count != 0;
        }
        
        public Lexer.Token Peek()
        {
            return Tokens[_current];
        }

        public Lexer.Token Previous()
        {
            return Tokens[_current - 1];
        }

        public Lexer.Token Consume(string message, params Lexer.Token.TokenType[] types)
        {
            var line = Previous().Line;
            foreach (var type in types)
            {
                if (Check(type))
                {
                    return Advance();
                }
            }

            throw new ParseError(message, line);
        }

        public bool Match(params Lexer.Token.TokenType[] tokens)
        {
            foreach (var type in tokens)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        public bool Check(params Lexer.Token.TokenType[] type)
        {
            if (IsAtEnd() && !type.Contains(Lexer.Token.TokenType.EOF))
            {
                return false;
            }

            return type.Contains(Peek().Type);
        }
        
        public Lexer.Token Advance()
        {
            if (!IsAtEnd())
            {
                _current += 1;
            }

            return Previous();
        }

        public void Synchronize()
        {
            Advance();

            while (!IsAtEnd() && Previous().Type != Lexer.Token.TokenType.NEWLINE)
            {
                Advance();
            }
        }

        public bool IsAtEnd()
        {
            return Peek().Type == Lexer.Token.TokenType.EOF;
        }
    }
}
