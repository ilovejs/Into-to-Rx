using System;

namespace Intro2Rx
{
    public class ConsoleColor : IDisposable
    {
        private readonly System.ConsoleColor _previousColor;
        public ConsoleColor(System.ConsoleColor color)
        {
            _previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }
        public void Dispose()
        {
            Console.ForegroundColor = _previousColor;
        }
    }
}