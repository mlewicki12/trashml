
using System;
using System.Collections.Generic;

namespace TrashML
{
    public class Parser
    {
        public class ParseError : Exception
        {
            public ParseError(string msg) : base(msg)
            {
            }
        }

        public List<Lexer.Token> Tokens;
        public List<ParseError> Errors;

        private int _current = 0;

        public Parser() : this(new List<Lexer.Token>())
        {
        }

        public Parser(List<Lexer.Token> tokens)
        {
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
            var statements = new List<Stmt>();

            try
            {
                while (!isAtEnd())
                {
                    Stmt stmt = statement();
                    if (stmt != null)
                    {
                        statements.Add(stmt);
                    }
                }

            }
            catch (ParseError e)
            {
                Errors.Add(e);
            }

            return statements;
        }
        
        public bool Error()
        {
            return Errors.Count != 0;
        }

        Stmt statement()
        {
            try
            {
                if (match(Lexer.Token.TokenType.DO)) return new Stmt.Block(block());
                if (match(Lexer.Token.TokenType.LET)) return assignment();
                if (match(Lexer.Token.TokenType.REPEAT)) return repeat();
                if (match(Lexer.Token.TokenType.PLAYER)) return dotted();
                if (match(Lexer.Token.TokenType.IF)) return ifstmt();
                if (match(Lexer.Token.TokenType.MACRO)) return macro();
                if (match(Lexer.Token.TokenType.PRINT)) return print();
                if (match(Lexer.Token.TokenType.NEWLINE)) return null; // kill any newlines

                return new Stmt.Expression(condition());
            }
            catch (ParseError e)
            {
                Errors.Add(e);
                synchronize();
            }

            // if we're here, we errored out
            return null;
        }

        List<Stmt> block()
        {
            // should get rid of pesky newlines?
            while (match(Lexer.Token.TokenType.NEWLINE))
            {
            }

            List<Stmt> statements = new List<Stmt>();

            while (!check(Lexer.Token.TokenType.END) && !isAtEnd())
            {
                var stmt = statement();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
            }

            consume("Expected 'end' after 'do'", Lexer.Token.TokenType.END);
            return statements;
        }

        Stmt assignment()
        {
            Lexer.Token name = consume("Expected identifier after 'let'", Lexer.Token.TokenType.IDENTIFIER);

            Expr initialiser = null;
            if (match(Lexer.Token.TokenType.EQUAL))
            {
                initialiser = condition();
            }

            consume("Expected new line after variable declaration", Lexer.Token.TokenType.NEWLINE,
                Lexer.Token.TokenType.EOF);
            return new Stmt.Assign(name, initialiser);
        }

        Stmt repeat()
        {
            Expr left = condition();
            Expr right = null;

            // hey we've got a high low loop
            // also im exhausted
            if (match(Lexer.Token.TokenType.COLON, Lexer.Token.TokenType.TO))
            {
                right = condition();
            }

            Stmt.Block blk = null;
            if (match(Lexer.Token.TokenType.DO)) blk = new Stmt.Block(block());
            else throw new ParseError("Expected block after repeat statement");

            if (right != null)
            {
                return new Stmt.Repeat(left, right, blk);
            }

            return new Stmt.Repeat(left, blk);
        }

        Stmt dotted()
        {
            if (match(Lexer.Token.TokenType.DOT))
            {
                if (match(Lexer.Token.TokenType.IDENTIFIER))
                {
                    var operation = previous();

                    if (match(Lexer.Token.TokenType.LEFT_PAREN))
                    {
                        var right = arguments();
                        return new Stmt.Dotted(operation, right);
                    }

                    throw new ParseError("Expected arguments after dotted call");
                }

                throw new ParseError("Expected identifier in dotted call");
            }

            throw new ParseError("Expected '.' after 'Player'");
        }

        Stmt ifstmt()
        {
            var cond = condition();

            consume("Expected 'do' after 'if'", Lexer.Token.TokenType.DO);
            var blk = block();
            List<Stmt> blk2 = null;

            if (match(Lexer.Token.TokenType.ELSE))
            {
                consume("Expected 'do' after 'else'", Lexer.Token.TokenType.DO);
                blk2 = block();
            }

            return new Stmt.If(cond, new Stmt.Block(blk), (blk2 != null) ? new Stmt.Block(blk2) : null);
        }

        Stmt macro()
        {
            consume("Expected identifier after macro definition", Lexer.Token.TokenType.IDENTIFIER);
            var id = previous();

            consume("Expected block after macro identifier", Lexer.Token.TokenType.DO);

            return new Stmt.Macro(id, new Stmt.Block(block()));
        }

        Stmt print()
        {
            var expr = condition();
            
            return new Stmt.Print(expr);
        }

        Expr condition()
        {
            Expr left = addition();

            if (match(Lexer.Token.TokenType.EQUAL, Lexer.Token.TokenType.BANG_EQUAL,
                Lexer.Token.TokenType.LESS, Lexer.Token.TokenType.LESS_EQUAL,
                Lexer.Token.TokenType.GREATER, Lexer.Token.TokenType.GREATER_EQUAL))
            {
                Lexer.Token op = previous();
                Expr right = addition();
                return new Expr.Binary(left, op, right);
            }

            return left;
        }

        Expr grouping()
        {
            Expr expr = condition();

            consume("Expected ')' after '('", Lexer.Token.TokenType.RIGHT_PAREN);
            return new Expr.Grouping(expr);
        }

        Expr arguments()
        {
            // no parameters inside
            if (match(Lexer.Token.TokenType.RIGHT_PAREN))
            {
                return new Expr.Grouping(null);
            }

            return new Expr.Grouping(grouping());
        }

        Expr unary()
        {
            if (match(Lexer.Token.TokenType.LEFT_PAREN)) return grouping();
            if (match(Lexer.Token.TokenType.BANG, Lexer.Token.TokenType.MINUS))
                return new Expr.Unary(previous(), unary());

            return primary();

        }

        Expr primary()
        {
            if (match(Lexer.Token.TokenType.FALSE)) return new Expr.Literal(false);
            if (match(Lexer.Token.TokenType.TRUE)) return new Expr.Literal(true);

            if (match(Lexer.Token.TokenType.NUMBER)) return new Expr.Literal(int.Parse(previous().Literal));
            if (match(Lexer.Token.TokenType.IDENTIFIER)) return new Expr.Variable(previous());

            throw new ParseError("Expecting expression");
        }

        Expr addition()
        {
            Expr expr = multiplication();

            while (match(Lexer.Token.TokenType.PLUS, Lexer.Token.TokenType.MINUS))
            {
                Lexer.Token op = previous();
                Expr right = multiplication();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        Expr multiplication()
        {
            Expr expr = unary();

            while (match(Lexer.Token.TokenType.MULTIPLY, Lexer.Token.TokenType.DIVIDE))
            {
                Lexer.Token op = previous();
                Expr right = unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        Lexer.Token peek()
        {
            return Tokens[_current];
        }

        Lexer.Token previous()
        {
            return Tokens[_current - 1];
        }

        Lexer.Token consume(string message, params Lexer.Token.TokenType[] types)
        {
            foreach (var type in types)
            {
                if (check(type))
                {
                    return advance();
                }
            }

            throw new ParseError(message);
        }

        bool match(params Lexer.Token.TokenType[] tokens)
        {
            foreach (var type in tokens)
            {
                if (check(type))
                {
                    advance();
                    return true;
                }
            }

            return false;
        }

        Lexer.Token advance()
        {
            if (!isAtEnd())
            {
                _current += 1;
            }

            return previous();
        }

        void synchronize()
        {
            advance();

            while (!isAtEnd() && !(previous().Type == Lexer.Token.TokenType.NEWLINE))
            {
                advance();
            }
        }

        bool isAtEnd()
        {
            return peek().Type == Lexer.Token.TokenType.EOF;
        }

        bool check(Lexer.Token.TokenType type)
        {
            if (isAtEnd() && type != Lexer.Token.TokenType.EOF)
            {
                return false;
            }

            return peek().Type == type;
        }
    }
}
