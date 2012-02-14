using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ASLib;

/*
 * ASLib Test Harness - xkcd module
 * 
 * SQA Coursework project (Computing - Adv. Higher)
 * Robert Tully (SCN 062807091)
 * 
 * Notice:  ASLib will be open-sourced post-examinations in Firrhill High School
 *          exam diets 2012 and 2013.
 *          
 *          These notices will be REMOVED from the final OSS version, which
 *          will be hosted at https://github.com/AerSunstrike
 */

namespace ASLibTestHarness
{
    class xkcdTests
    {
        private string output;

        public void run() // Start function called from the harness core
        {
            // Setup
            Console.Write("Instantiating instance of ASComicAccess.xkcd... ");
            ASComicAccess.xkcd testClass = new ASComicAccess.xkcd();
            Console.WriteLine("Done.\n");

            // Test of the get-latest function
            Console.Write("Testing latest comic function... ");
            if (testLatestComic(testClass))
            {
                // Note: All this guff isn't technically neccessary, but it's personal preference
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkGreen; // GREEN pass text
                Console.Write("PASS");
                Console.ResetColor(); // Back to normal colour
                Console.WriteLine("]");
                Console.WriteLine("TEST OUTPUT:");
                Console.WriteLine(output);
            }
            else
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkRed; // RED fail text
                Console.Write("FAIL");
                Console.ResetColor(); // Back to normal colour
                Console.WriteLine("]");
            }

            // Test of get-by-ID function
            int startID = 612;
            int endID = 618;

            Console.WriteLine("Testing comic by ID function with IDs " + startID.ToString() + " to " + endID.ToString() +"... ");
            for (int i = startID; i <= endID; i++)
            {
                Console.Write("\nTesting with ID " + i.ToString() + "... ");
                if (testComicByID(testClass, i))
                {
                    // Note: All this guff isn't technically neccessary, but it's personal preference
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkGreen; // GREEN pass text
                    Console.Write("PASS");
                    Console.ResetColor(); // Back to normal colour
                    Console.WriteLine("]");
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output);
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkRed; // RED fail text
                    Console.Write("FAIL");
                    Console.ResetColor(); // Back to normal colour
                    Console.WriteLine("]");
                }
            }
        }

        private bool testLatestComic(ASComicAccess.xkcd testClass) // Get latest comic data test
        {
            ASComicAccess.xkcd.ComicMetadata metadata;

            try
            {
                metadata = testClass.getComic();
            }
            catch
            {
                return false; // Test failed.
            }

            output = ("\n         ID (string): " + (string)metadata.num + "\nComic Title (string): " + (string)metadata.safe_title + "\n"); // Generate output
            return true;
        }

        private bool testComicByID(ASComicAccess.xkcd testClass, int ID) // Get comic by ID test
        {
            ASComicAccess.xkcd.ComicMetadata metadata;

            try
            {
                metadata = testClass.getComic(ID);
            }
            catch
            {
                return false; // Test failed.
            }

            output = ("\n         ID (string): " + (string)metadata.num + "\nComic Title (string): " + (string)metadata.safe_title + "\n"); // Generate output
            return true;
        }
    }
}
