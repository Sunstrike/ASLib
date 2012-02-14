using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

/*
 * ASLib Test Harness Core
 * 
 * Available Tests:
 *      - ASComicAccess
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
    class Core
    {
        static void Main(string[] args)
        {
            string input = "0"; // Yes, strings. This was causing trouble with ReadLine, and it's simpler than parsing to int. Memory use isn't a concern.
            int doBreak = 0;

            do
            {
                input = GetChoice();
                if (input == "1" /*|| input == "2"*/) {
                    doBreak = 1;
                    try
                    {
                        Console.Clear(); // Apparently MS say this has to be try..catch'ed due to breaking on writing to file.
                    }
                    catch
                    {
                        // Do nuthin'
                    }
                } else {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    
                    try
                    {
                        Console.Clear(); // Apparently MS say this has to be try..catch'ed due to breaking on writing to file.
                    }
                    catch
                    {
                        // Do nuthin'
                    }
                }
            } while (doBreak == 0);

            switch (input)
            {
                case "1":
                    xkcdTestCase(); // To Xkcd Tests
                    break;
                default:
                    throw new NotImplementedException(); // Because why should we handle the impossible?
            }
        }

        private static void xkcdTestCase()
        {
            throw new NotImplementedException(); // TODO: Replace with test() function of xkcd's testcase object
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
