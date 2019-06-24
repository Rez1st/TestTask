using System;

namespace App
{
    /// <summary>
    /// Pseudo Logger
    /// </summary>
    public static class Log
    {
        public static void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write("WARNING", msg);
        }

        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Write("INFO", msg);
        }

        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write("ERROR", msg);
        }

        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Write("SUCCESS", msg);
        }

        private static void Write(string level, string message)
        {
            Console.WriteLine($"{DateTime.Now.TimeOfDay} - {level} : {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}