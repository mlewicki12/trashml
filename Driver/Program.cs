
using System;
using System.Collections.Generic;
using System.IO;
using TrashML.Main;

namespace TrashML.Driver
{
    internal class Program
    {
        // should probably abstract this to TrashML.ReadFile or something
        public static void Main(string[] args)
        {
            string text;
            var error = false;
            
            if (args.Length >= 1)
            {
                text = ReadFile(args[0]);
                if (text.StartsWith("ERR"))
                {
                    Console.WriteLine(text);
                    error = true;
                }
            } else text = "let x = 5\nprint x";

            if (!error)
            {
                Lexer lexer = new Lexer(text);
                var tokens = lexer.Scan();

                if (!lexer.Error())
                {
                    Parser parser = new Parser(tokens);
                    
                    var parsed = parser.Parse();

                    if (!parser.Error())
                    {
                        Interpreter trashml = new Interpreter();
                        trashml.Interpret(parsed);

                        if (trashml.Error())
                        {
                            printErrors("TML: Errors encountered while interpreting code!\n", trashml.Errors);
                        }
                    } else
                    {
                        printErrors("TML: Errors encountered while parsing code!\n", parser.Errors);
                    }
                } else {
                    printErrors("TML: Errors encountered while scanning code!\n", lexer.Errors);
                }
            }
        }

        public static string ReadFile(string name)
        {
            try
            {
                return File.ReadAllText(name);
            }
            catch (IOException e)
            {
                return "ERR: " + e.Message;
            }
        }

        static void printErrors<T>(string msg, List<T> errors) where T : Exception
        {
            // TODO: make errors more informative
            Console.WriteLine(msg);
            foreach(var err in errors)
            {
                Console.WriteLine(err + "\n");
            }
        }
    }
}