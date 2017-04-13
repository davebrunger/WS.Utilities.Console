using System;

namespace WS.Utilities.Console
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string line, bool highlight)
        {
            WriteLine(line, highlight ? ConsoleColor.Cyan : System.Console.ForegroundColor);
        }

        public void WriteErrorLine(string line)
        {
            WriteLine(line, ConsoleColor.Red);
        }

        private void WriteLine(string line, ConsoleColor lineColor)
        {
            var oldColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = lineColor;
            System.Console.WriteLine(line);
            System.Console.ForegroundColor = oldColor;
        }
    }
}
