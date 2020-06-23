using System;

namespace Cslox
{
    public class Error
    {
        public static void ReportError(int line, int position, in String message)
        {
            Report(line, position, message);
        }

        private static void Report(in int line, int where, in string message)
        {
            Console.WriteLine($"[line {line}] Error {message} at position {where}");
        }
    }
}