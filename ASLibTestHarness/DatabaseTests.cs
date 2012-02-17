﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ASLib;

/*
 * ASLib Test Harness - Database module
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
    class DatabaseTests
    {
        public string output { get; set; }
        
        public void run() // Start function called from the harness core
        {
            string path = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + "\\AppData\\Roaming\\ASLibTestHarness\\TestDB.sdf"; // Test Harness DB file path
            //                        ^^^^^^^^^^^^^^^^^^^^^^^^^^ Grab user home path        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Append path to AppData folder and DB file

            // Setup
            Console.Write("Checking if target folder exists... ");
            string dirPath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + "\\AppData\\Roaming\\ASLibTestHarness";
            if (!Directory.Exists(dirPath))
            {
                Console.Write("no, creating folder... ");
                Directory.CreateDirectory(dirPath);
            }
            Console.WriteLine("Done.");

            Console.Write("Instantiating instance of ASComicDatabase... ");
            ASComicDatabase testClass = new ASComicDatabase(path);
            Console.WriteLine("Done.");

            output = ""; // Reset this, just in case
            Console.Write("\nTesting DB creation with path " + path + "... ");
            if (testDBCreation(testClass))
            {
                // Note: All this guff isn't technically neccessary, but it's personal preference
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkGreen; // GREEN pass text
                Console.Write("PASS");
                Console.ResetColor(); // Back to normal colour
                Console.WriteLine("]");
            }
            else
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkRed; // RED fail text
                Console.Write("FAIL");
                Console.ResetColor(); // Back to normal colour
                Console.WriteLine("]");
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
                Console.WriteLine("Press any key to crash...");
                Console.ReadLine();
                throw new IOException("Cannot continue without read/write access to DB file!");
            }

            Console.Write("\nTesting Data insertion (for metadata)... ");
            if (testInsertMetadata(testClass))
            {
                // Note: All this guff isn't technically neccessary, but it's personal preference
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkGreen; // GREEN pass text
                Console.Write("PASS");
                Console.ResetColor(); // Back to normal colour
                Console.WriteLine("]");
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
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

        private bool testDBCreation(ASComicDatabase engine) // Test the createDbFile function
        {
            try
            {
                engine.createDbFile();
            }
            catch (System.IO.IOException e)
            {
                output = "IOException: Process could not access file.\n\nFULL TRACE:\n" + e.ToString();
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("UNKNOWN EXCEPTION CAUGHT FROM DB ENGINE:\n\n" + e.ToString()); // Log exception to debug channel
                return false; // Fail test
            }
            return true; // Pass test
        }

        private bool testInsertMetadata(ASComicDatabase engine) // Test the insertRow function
        {
            throw new NotImplementedException();
        }

        private bool testInsertImageData(ASComicDatabase engine) // Test updateImgData function
        {
            throw new NotImplementedException();
        }

        private bool testDeleteImageData(ASComicDatabase engine) // Test deleteImgData function
        {
            throw new NotImplementedException();
        }
    }
}
