
using System.IO;
using TrashML.Main;
using TrashML.Objects;

namespace TrashML.Elements
{
    public static class RequireExtension
    {
        public static Stmt Require(this Parser parser)
        {
            return new Stmt.Require(parser.Primary());
        }

        public static TrashObject RequireStmt(this Interpreter interpreter, Stmt.Require stmt)
        {
            var val = interpreter.Evaluate(stmt.File).Access();

            if (val is string) {
                var lines = readFile(val as string);
                if (lines.StartsWith("ERR:")) 
                {
                    throw new Interpreter.RuntimeError($"Unable to read {val}, received error\n{lines}");
                }
                
                Lexer lexer = new Lexer(lines);
                var tokens = lexer.Scan();

                while (!lexer.Error()) // break out of it if we get an error
                {
                    Parser parser = new Parser(tokens);
                    
                    var parsed = parser.Parse();

                    if (parser.Error()) {
                        break;
                    }
                    
                    interpreter.Interpret(parsed);
                    if (interpreter.Error()) 
                    {
                        break;
                    }

                    return null; // kill the loop if we didn't hit any errors
                }
                
                throw new Interpreter.RuntimeError($"Error parsing provided file {val}");

            } 
            
            throw new Interpreter.RuntimeError("Invalid argument to require");
        }
        
        private static string readFile(string file) 
        {
            try
            {
                return File.ReadAllText(file);
            }
            catch (IOException e)
            {
                return "ERR: " + e.Message;
            }
        }
    }
}