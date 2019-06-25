
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace TrashML.Main
{
    public class Lexer
    {
        // error codes could be an idea?
        // also a general better handling of errors lol
        public class ScanError : Exception
        {
            private int line;
            
            public ScanError(string msg, int line) : base(msg)
            {
                // workaround, because lines are indexed from 0
                this.line = line + 1;
            }

            public override string ToString()
            {
                return $"TrashML Scan Error, line {line}: {Message}";
            }
        }

        private readonly Dictionary<string, Token.TokenType> _keywords;

        private readonly string _source;
        private int _start;
        private int _current;
        private int _line;
        
        public List<Token> Tokens;
        public List<ScanError> Errors;

        public Lexer(string expr)
        {
            _source = expr;

            _start = 0;
            _current = 0;
            _line = 0;

            Tokens = new List<Token>();
            Errors = new List<ScanError>();

            // add keywords
            _keywords = new Dictionary<string, Token.TokenType>();
            
            _keywords.Add("repeat", Token.TokenType.REPEAT);
            _keywords.Add("require", Token.TokenType.REQUIRE);
            _keywords.Add("let", Token.TokenType.LET);
            _keywords.Add("if", Token.TokenType.IF);
            _keywords.Add("else", Token.TokenType.ELSE);
            _keywords.Add("macro", Token.TokenType.MACRO);
            _keywords.Add("function", Token.TokenType.MACRO);
            _keywords.Add("do", Token.TokenType.DO);
            _keywords.Add("to", Token.TokenType.TO);
            _keywords.Add("end", Token.TokenType.END);
            _keywords.Add("print", Token.TokenType.PRINT);
            _keywords.Add("false", Token.TokenType.FALSE);
            _keywords.Add("true", Token.TokenType.TRUE);
            _keywords.Add("and", Token.TokenType.AND);
            _keywords.Add("or", Token.TokenType.OR);
            _keywords.Add("class", Token.TokenType.CLASS);
            _keywords.Add("define", Token.TokenType.DEFINE);
            _keywords.Add("member", Token.TokenType.MEMBER);
            _keywords.Add("return", Token.TokenType.RETURN);
            _keywords.Add("new", Token.TokenType.NEW);
            _keywords.Add("with", Token.TokenType.WITH);
        }

        public class Token
        {
            public enum TokenType
            {
                NUMBER, IDENTIFIER, STRING,
                
                PLUS, MINUS, MULTIPLY, DIVIDE,
                EQUAL, BANG, 
                EQUAL_EQUAL, BANG_EQUAL, 
                
                COMMA, COLON, DOT,
                
                LESS_EQUAL, LESS,
                GREATER_EQUAL, GREATER,
                
                LEFT_PAREN, RIGHT_PAREN,
                
                REPEAT, REQUIRE, IF, ELSE,
                MACRO, DO, END, PRINT,
                TO, LET, TRUE, FALSE, AND, OR,
                CLASS, DEFINE, MEMBER, RETURN,
                NEW, WITH,
                
                NEWLINE, EOF
            }

            public TokenType Type;
            public int Line;
            public string Literal;

            public override string ToString()
            {
                if (Type == TokenType.NEWLINE)
                {
                    // I don't wanna print newlines
                    return $"Token [{Type}]";
                }
                
                return $"Token [{Type}]: {Literal}";
            }
        }

        public List<Token> Scan()
        {
            Tokens = new List<Token>();
            Errors = new List<ScanError>();
            
            _start = 0;
            _current = 0;
            _line = 0;

            while (!isAtEnd())
            {
                try
                {
                    _start = _current;
                    scanToken();
                } catch (ScanError e)
                {
                    Errors.Add(e);
                }
            }

            addToken(Token.TokenType.EOF, "");
            return Tokens;
        }

        void scanToken()
        {
            char c = advance();

            switch (c)
            {
                // these are not the characters you're looking for
                case ' ':
                case '\r':
                case '\t':
                    break;

                case '\n':
                    // crush empty lines down to one new line
                    while (peek() == '\n')
                    {
                        _line += 1;
                        advance();
                    }

                    _line += 1;
                    addToken(Token.TokenType.NEWLINE, "\n");
                    break;

                // single character tokens
                case '+':
                    addToken(Token.TokenType.PLUS, "+");
                    break;

                case '-':
                    addToken(Token.TokenType.MINUS, "-");
                    break;

                case '*':
                    addToken(Token.TokenType.MULTIPLY, "*");
                    break;

                case '/':
                    addToken(Token.TokenType.DIVIDE, "/");
                    break;

                case ':':
                    addToken(Token.TokenType.COLON, ":");
                    break;

                case '.':
                    addToken(Token.TokenType.DOT, ".");
                    break;
                
                case ',':
                    addToken(Token.TokenType.COMMA, ",");
                    break;

                case '=':
                    if (peek() == '=')
                    {
                        addToken(Token.TokenType.EQUAL_EQUAL, "==");
                        advance();
                    }
                    else addToken(Token.TokenType.EQUAL, "=");
                    
                    break;

                case '<':
                    if (peek() == '=')
                    {
                        addToken(Token.TokenType.LESS_EQUAL, "<=");
                        advance();
                    }
                    else addToken(Token.TokenType.LESS, "<");

                    break;

                case '>':
                    if (peek() == '=')
                    {
                        addToken(Token.TokenType.GREATER_EQUAL, ">=");
                        advance();
                    }
                    else addToken(Token.TokenType.GREATER, ">");

                    break;

                case '(':
                    addToken(Token.TokenType.LEFT_PAREN, "(");
                    break;

                case ')':
                    addToken(Token.TokenType.RIGHT_PAREN, ")");
                    break;

                case '!':
                    if (peek() == '=')
                    {
                        addToken(Token.TokenType.BANG_EQUAL, "!=");
                        advance();
                    }
                    else addToken(Token.TokenType.BANG, "!");

                    break;
                
                case '"':
                    strng();
                    break;
                
                case '#':
                    comment();
                    break;

                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        identifier();
                    }
                    else
                    {
                        throw new ScanError($"Invalid character {c} found in input!", _line);
                    }

                    break;
            }
        }

        public bool Error()
        {
            return Errors.Count != 0;
        }

        void addToken(Token.TokenType toAdd, string lit)
        {
            Tokens.Add(new Token {Type = toAdd, Line = _line, Literal = lit});
        }

        void number()
        {
            while (isDigit(peek()))
            {
                advance();
            }

            addToken(Token.TokenType.NUMBER, _source.Substring(_start, _current - _start));
        }

        void identifier()
        {
            var c = peek();
            
            while (isAlphaNumeric(c))
            {
                advance();
                c = peek();
            }

            string text = _source.Substring(_start, _current - _start);
            if (_keywords.ContainsKey(text))
            {
                addToken(_keywords[text], text);
            }
            else
            {
                addToken(Token.TokenType.IDENTIFIER, text);
            }
        }
        
        void strng()
        {
            var c = advance();
            while (c != '"')
            {
                if (isAtEnd())
                {
                    throw new ScanError("Expected ending '\"' when defining string value", _line);
                }

                c = advance();
            }
            
            string text = _source.Substring(_start + 1, _current - _start - 2);
            addToken(Token.TokenType.STRING, text);
        }

        void comment()
        {
            while (!isAtEnd() && advance() != '\n')
            {
            }

            _line += 1;
        }

        char advance()
        {
            return _source[_current++];
        }

        char peek()
        {
            if (isAtEnd())
            {
                return '\0';
            }

            return _source[_current];
        }

        bool isDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        bool isAlpha(char c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_'));
        }

        bool isAlphaNumeric(char c)
        {
            return isDigit(c) || isAlpha(c);
        }

        bool isAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}