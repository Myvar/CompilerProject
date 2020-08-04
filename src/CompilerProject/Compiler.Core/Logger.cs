using System;
using System.Diagnostics;

namespace Compiler.Core
{
    public static class Logger
    {
        public static void Debug(string s)
        {

            try
            {
                var stackTrace = new StackTrace();
                var frame = stackTrace.GetFrame(1);
                var meth = frame.GetMethod();

                var args = "";

                foreach (var info in meth.GetParameters())
                {
                    args += info.Name + ":" + info.ParameterType.Name + ", ";
                }

                s = "[" + meth.DeclaringType.Name + "::" + meth.Name + "(" +
                    args.Trim().Trim(',') + ")] " + s;
            }
            catch (Exception)
            {
            }
            

            
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Debug");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            

            Console.WriteLine(s);
            
            Console.ResetColor();
        }
        
        public static void Log(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Log");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(s);
        }
        
        public static void Warn(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Warn");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(s);
        }
        
        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(s);
        }
    }
}