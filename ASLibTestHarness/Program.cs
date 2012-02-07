using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASLibTestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "0";
            int doBreak = 0;

            do
            {
                input = GetChoice();
                if (input == "1" /*|| input == "2"*/) {
                    doBreak = 1;
                } else {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    Thread.Sleep(1000);
                }
            } while (doBreak == 0);
        }

        private static string GetChoice()
        {
            Console.WriteLine("TEST HARNESSES:");
            Console.WriteLine(" 1. ASComicAccess");
            Console.WriteLine();
            Console.Write("Enter option: ");
            return Console.ReadLine();
        }
    }
}
