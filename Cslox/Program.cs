using System;
using System.Collections.Generic;
using System.IO;

namespace Cslox
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: Cslox.exe [script]");
                return;
            }

            if (args.Length == 1)
                RunFile(args[0]);
            else
                RunPrompt();
        }

        private static void RunPrompt()
        {
            Console.Clear();
            while (true)
            {
                Console.Write(">");
                var sourceCode = Console.ReadLine();
                Run(sourceCode);
            }
        }

        private static void RunFile(in String s)
        {
            var sourceCode = File.ReadAllText(s);
            Run(sourceCode);
        }

        private static void Run(in string source)
        {
            Scanner scanner = new Scanner(source);
            IEnumerable<Token> tokens = scanner.ScanTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}