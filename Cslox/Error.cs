using System;

namespace Cslox
{
    public class Error
    {
        public static void ReportError(in string line, int lineNumber, int position, in String message)
        {
            Report(line, lineNumber, position, message);
        }

        private static void Report(in string line, int lineNumber, int where, in string message)
        {
            Console.WriteLine($"Error {message} [line {lineNumber} at position {where}]");
            Console.WriteLine(line);
            Console.CursorLeft = where;
            Console.WriteLine("^");
        }
    }
}