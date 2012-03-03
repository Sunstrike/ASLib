using System;
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
            ConsoleServices OutputManager = new ConsoleServices();
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
                OutputManager.writePassText();
            }
            else
            {
                OutputManager.writeFailText();
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
                Console.WriteLine("Press any key to crash...");
                Console.ReadLine();
                throw new IOException("Cannot continue without read/write access to DB file!");
            }

            output = "";
            Console.Write("\nTesting Data insertion (for metadata)... ");
            if (testInsertMetadata(testClass))
            {
                OutputManager.writePassText();
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
            }
            else
            {
                OutputManager.writeFailText();
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
            }

            output = "";
            Console.Write("\nTesting Data retrieval (for metadata)... ");
            if (testReadMetadata(testClass))
            {
                OutputManager.writePassText();
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
            }
            else
            {
                OutputManager.writeFailText();
                if (output != "")
                {
                    Console.WriteLine("TEST OUTPUT:");
                    Console.WriteLine(output + "\n");
                }
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
            try
            {
                engine.insertRow(1023, "http://imgs.xkcd.com/comics/late_night_pbs.png", "Late-Night PBS", "Then it switched to these old black-and-white tapes of Bob Ross slumped against the wall of an empty room, painting the least happy trees you've ever seen. Either PBS needs to beef up studio security or I need to stop using Ambien to sleep.", "[[Scruffy is rubbing sleep out of their eyes and talking to clean shaven.]]\nScruffy: Have you ever watched PBS late at night?\nScruffy: I fell asleep after \nDownton\n and woke up at like 3 AM.\n\n[[The upper portion of the panel continues dialogue, while the lower shows a drunk gameshow host and several contestants.  The monitor shows a field of crosses, presumably graves.]]\nScruffy: \nWhere in the World is Carmen Sandiego\n was back on, except the host hadn't aged well and he'd clearly been drinking.\nScruffy: Every question took them to some horrible place like Mogadishu or the Cambodian killing fields.\n\n[[Now it shows a bookshelf revealing a hidden room.]]\nScruffy: The kids were freaked out, but they kept playing.  Eventually they were told they'd found Carmen Sandiego hiding behind a bookshelf in a Dutch apartment.\n\nScruffy: The Chief appeared and asked \"Are you \nproud\n of what you've become?\"\nScruffy: Then Rockapella walked out and just glared at the kids until they started crying.\nClean-shaven: I, uh, don't remember the old show being that dark.\nScruffy: Maybe we were too young to pick up on it.\n\n{{Title text: Then it switched to these old black-and-white tapes of Bob Ross slumped against the wall of an empty room, painting the least happy trees you've ever seen. Either PBS needs to beef up studio security or I need to stop using Ambien to sleep.}}", "29:02:2012");
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool testReadMetadata(ASComicDatabase engine) // Test getRow function
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            
            try
            {
                dict = engine.getRow(1023);
            }
            catch
            {
                return false; // Query failed
            }

            if (dict.ContainsKey("safe_title") && dict.ContainsValue("Late-Night PBS"))
            {
                return true; // Verified there is actually valid data coming back
            }
            return false;
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
