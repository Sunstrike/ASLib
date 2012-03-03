using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASLibTestHarness
{
    class ConsoleServices
    {
        public void writeFailText()
        {
            Console.Write("[");
            writeRedText("FAIL");
            Console.WriteLine("]");
        }

        public void writePassText()
        {
            Console.Write("[");
            writeGreenText("PASS");
            Console.WriteLine("]");
        }

        public void writeRedText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; // RED fail text
            Console.Write(text);
            Console.ResetColor(); // Back to normal colour
        }

        public void writeGreenText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen; // GREEN pass text
            Console.Write(text);
            Console.ResetColor(); // Back to normal colour
        }
    }
}
