using System;

namespace Cslox
{
    public class Error
    {
        static void ReportError(int line, in String message)
        {
            Report(line, "", message);
        }

        private static void Report(in int line, string where, in string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
        }
    }
}