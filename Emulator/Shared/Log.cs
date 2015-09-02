using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Log
    {
        private static object section = new object();
        public static void WriteLine(string str)
        {
            writeline(str, ConsoleColor.Green);
        }

        public static void WriteError(string str)
        {
            writeline(str, ConsoleColor.DarkRed);
        }

        public static void WriteDebug(string str)
        {
            writeline(str, ConsoleColor.DarkMagenta);
        }

        public static void WriteBlank(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("");
            }
        }

        private static void writeline(string str, ConsoleColor c)
        {
            lock (section)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + DateTime.Now.ToString("hh:mm:ss.fff") + "] > ");
                Console.ForegroundColor = c;
                Console.Write(str);
                Console.WriteLine("");
            }
        }
    }
}
