using System;
using System.IO;
using System.Linq;

namespace Cslox
{
    internal static class Program
    {
        private static void Main(string[] args)
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

        private static void RunFile(in string s)
        {
            var sourceCode = File.ReadAllText(s);
            Run(sourceCode);
        }

        private static void Run(in string source)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var errors = scanner.GetErrors();
            if (errors.Any())
            {
                var foreground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var error in scanner.GetErrors())
                    Console.WriteLine(error);

                Console.ForegroundColor = foreground;
            }
            else
            {
                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }
            }
        }
    }
}