
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace TrashML
{
    public class Lexer
    {
        public class ScanError : Exception
        {
            public ScanError(string msg) : base(msg)
            {

            }
        }

        private Dictionary<string, Token.TokenType> Keywords;

        private string _source;
        private int _start;
        private int _current;
        
        public List<Token> Tokens;
        public List<ScanError> Errors;

        public Lexer(string expr)
        {
            _source = expr;

            _start = 0;
            _current = 0;

            Tokens = new List<Token>();
            Errors = new List<ScanError>();

            // add keywords
            Keywords = new Dictionary<string, Token.TokenType>();
            
            Keywords.Add("repeat", Token.TokenType.REPEAT);
            Keywords.Add("let", Token.TokenType.LET);
            Keywords.Add("if", Token.TokenType.IF);
            Keywords.Add("else", Token.TokenType.ELSE);
            Keywords.Add("macro", Token.TokenType.MACRO);
            Keywords.Add("do", Token.TokenType.DO);
            Keywords.Add("to", Token.TokenType.TO);
            Keywords.Add("end", Token.TokenType.END);
            Keywords.Add("print", Token.TokenType.PRINT);
            Keywords.Add("false", Token.TokenType.FALSE);
            Keywords.Add("true", Token.TokenType.TRUE);
        }

        public class Token
        {
            // TODO: organise this and make it look better
            public enum TokenType
            {
                NUMBER,
                IDENTIFIER,
                PLUS,
                MINUS,
                MULTIPLY,
                DIVIDE,
                EQUAL,
                BANG,
                BANG_EQUAL,
                LESS_EQUAL,
                LESS,
                GREATER_EQUAL,
                GREATER,
                LEFT_PAREN,
                RIGHT_PAREN,

                REPEAT,
                COLON,
                IF,
                ELSE,
                MACRO,
                EOF,
                DO,
                END,
                PRINT,
                DOT,
                TO,
                LET,
                NEWLINE,
                FALSE,
                TRUE,
                DOTTED
            };

            public TokenType Type;
            public string Literal;

            public override string ToString()
            {
                return $"Token [{Type}]: {Literal}";
            }
        }

        public List<Token> Scan()
        {
            Tokens = new List<Token>();
            _start = 0;
            _current = 0;

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
                // nothing to see here, move along
                case ' ':
                case '\r':
                case '\t':
                    break;

                case '\n':
                    // crush empty lines down to one new line
                    while (peek() == '\n')
                    {
                        advance();
                    }

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

                case '=':
                    addToken(Token.TokenType.EQUAL, "=");
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
                
               // case '"':
               //     string();
               //     break;
               // 
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
                        throw new ScanError("Invalid character found in input!");
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
            Tokens.Add(new Token {Type = toAdd, Literal = lit});
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
            var dotted = false;
            
            while (isAlphaNumeric(c))
            {
                advance();
                if (c == '.')
                {
                    dotted = true;
                }
                
                c = peek();
            }

            string text = _source.Substring(_start, _current - _start);
            if (Keywords.ContainsKey(text))
            {
                addToken(Keywords[text], text);
            }
            else
            {
                if (dotted)
                {
                    addToken(Token.TokenType.DOTTED, text);
                } else addToken(Token.TokenType.IDENTIFIER, text);
            }
        }

        void comment()
        {
            while (!isAtEnd() && advance() != '\n')
            {
            }
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
            return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_') | (c == '.'));
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