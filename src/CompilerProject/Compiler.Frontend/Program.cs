using System;
using System.IO;

namespace Compiler.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            //here we will load the grammer file and generate our code

            var grammerFile = File.ReadAllText("./Grammer/cp.grammer");
        }
    }
}