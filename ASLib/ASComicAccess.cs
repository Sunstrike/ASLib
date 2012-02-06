using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

/*
 * ASComicAccess
 * 
 * Supports:
 *      - xkcd
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


namespace ASLib
{
    public class ASComicAccess
    {
        public class xkcd
        {
            public class ComicMetadata // Used for deserializing JSON data
            {
                string num { get; set; }
                string img { get; set; }
                string safe_title { get; set; }
                string day { get; set; }
                string month { get; set; }
                string year { get; set; }
                string transcript { get; set; }
                string alt { get; set; }
            } 

            static private ComicMetadata __parseXkcdJSON(string jsonData) // Parses JSON data provided as a string into a new ComicMetadata object
            {
                JavaScriptSerializer parser = new JavaScriptSerializer();
                try
                {
                    return parser.Deserialize<ComicMetadata>(jsonData); // Return the ComicMetadata object to caller
                }
                catch
                {
                    Debug.WriteLine("ERROR CAUGHT IN FUNCTION __parseXkcdJSON(string jsonData)!");
                    Debug.Fail("ASLib: Error caught in Json Parser", "Error occured in class ASComicAccess in function __parseXkcdJSON(string jsonData).");
                    return null; // Couldn't parse for some reason (null input?) -> Return null
                }
            }

            static private string __getXkcdJSON(int id)
            {
                WebClient connectAgent = new WebClient();
                string returnValue = "";

                try
                {
                    byte[] rawData = connectAgent.DownloadData("http://xkcd.com/"+id+"/info.0.json");
                    returnValue = Encoding.Unicode.GetString(rawData);
                } catch (WebException) {
                    Debug.WriteLine("Could not download file.");
                    returnValue = "EE";
                }

                return returnValue;
            }

            static private string __getLatestXkcdJSON()
            {
                WebClient connectAgent = new WebClient();
                string returnValue = "";

                try
                {
                    byte[] rawData = connectAgent.DownloadData("http://xkcd.com/info.0.json");
                    returnValue = Encoding.Unicode.GetString(rawData);
                }
                catch (WebException)
                {
                    Debug.WriteLine("Could not download file.");
                    returnValue = "EE";
                }

                return returnValue;
            }

            public ComicMetadata getComic(int ID) // Get by ID
            {
                string JSON = __getXkcdJSON(ID); // Get xkcd JSON file by ID
                if (JSON == "EE")
                {
                    // Couldn't D/L file!
                    throw new WebException();
                }
                return __parseXkcdJSON(JSON); // Parse and return data
            }

            public ComicMetadata getComic() // Blindly get latest comic
            {
                string JSON = __getLatestXkcdJSON(); // Get latest xkcd JSON file
                if (JSON == "EE")
                {
                    // Couldn't D/L file!
                    throw new WebException();
                }
                return __parseXkcdJSON(JSON); // Parse and return data
            }
        }
    }
}
