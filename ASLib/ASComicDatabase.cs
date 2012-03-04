using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data.SqlServerCe;
using System.Diagnostics;

/*
 * ASComicDatabase
 * 
 * Convenience class to Microsoft SQL Server Compact to manage a comic database instance.
 * NOTE: Currently only compatible with xkcd as parsed by ASComicAccess.
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
    public class ASComicDatabase
    {
        // Object instance variables
        string dbPath;

        public ASComicDatabase(string path)
        {
            this.dbPath = Path.GetFullPath(path); // If this fails it'll throw an exception for us anyway - No need for Try..Catch block
        }

        public void createDbFile()
        {
            if (File.Exists(dbPath)) // Remove the file if it already exists (like for Jettison)
            {
                File.Delete(dbPath);
            }

            string connString = @"Data Source = " + dbPath; // Create connection string for SQLCE DB
            SqlCeEngine engine = new SqlCeEngine(connString); // Instantiate engine object

            engine.CreateDatabase(); // Create DB file!

            this.__initDatabase(connString);
        }

        private void __initDatabase(string connStr)
        {
            // Create tables/columns (Using code from Stack Overflow)
            using (SqlCeConnection conn = new SqlCeConnection(connStr))
            {
                
                /* SOME NOTES ON THE FOLLOWING
                 * ---------------------------
                 * We use ntext for dates: The 'datetime' type is accurate down to milliseconds, while we only have day accuracy (I would use an ASCII field, but SQLCE is Unicode-only (Damnit i18n))
                 * We use smallint for num: These numbers will never (within a reasonable timescale) go above 32767, thus we use a smaller variable to save space
                 * We use ntext for text fields: nvarchar is limited to 255 characters and uses the same storage space as ntext it seems from MSDN documentation
                 * 
                 * date format: DD:MM:YYYY
                 * This is used for easy reparsing back into day, month and year consistent with the JSON metadata
                 */
                using (SqlCeCommand cmd = new SqlCeCommand(@"CREATE TABLE comics (num smallint, img ntext, img_data image, safe_title ntext, alt ntext, transcript ntext, date ntext)", conn))
                {
                    try
                    {
                        conn.Open(); // Establish connection to database (Why do embedded servers need connections? It's to a file!)
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery(); // Run init query on database
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("EXCEPTION IN __initDatabase!\n" + ex.ToString()); // Log it...
                        throw; // ...and fail.
                    }
                }
            }
        }

        public bool verifyDatabase() // Calls SQLCE's verification function
        {
            string connString = @"Data Source = " + dbPath; // Create connection string for SQLCE DB
            SqlCeEngine engine = new SqlCeEngine(connString); // Instantiate engine object

            if (!engine.Verify())
            {
                throw new Exception("Database corrupt"); // Host application should use a try..catch block to catch this
            }

            return true; // Return true as Database is intact
        }

        public void compactDatabase() // Calls SQLCE's compactor
        {
            string connString = @"Data Source = " + dbPath; // Create connection string for SQLCE DB
            SqlCeEngine engine = new SqlCeEngine(connString); // Instantiate engine object

            // Do the compaction
            engine.Compact(null);
            // We then verify the database, just incase.
            if (!engine.Verify())
            {
                throw new Exception("Database corrupt");
            }
        }

        public void insertRow(int num, string img, string safe_title, string alt, string transcript, string date) // NOT including image DATA, as that's not 'compulsory' data
        {
            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source = " + dbPath)) // DB connection
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"INSERT comics (num, img, safe_title, alt, transcript, date) VALUES (@num, @img, @safe_title, @alt, @transcript, @date)")) // SQL command
                {
                    // Add C# data to the SQL query
                    cmd.Parameters.AddWithValue(@"@num", num);
                    //cmd.Parameters.Add(new SqlCeParameter("@num", num));
                    cmd.Parameters.AddWithValue(@"@img", img);
                    cmd.Parameters.AddWithValue(@"@safe_title", safe_title);
                    cmd.Parameters.AddWithValue(@"@alt", alt);
                    cmd.Parameters.AddWithValue(@"@transcript", transcript);
                    cmd.Parameters.AddWithValue(@"@date", date);

                    conn.Open(); // Open DB connection
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery(); // Run query
                }
            }
        }

        public Dictionary<string,string> getRow(int num) // NOT including image DATA, as that's not 'compulsory' data
        {
            string img, safe_title, alt, transcript, date;
            img = "";
            safe_title = "";
            alt = "";
            transcript = "";
            date = "";
            
            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source = " + dbPath)) // DB connection
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"SELECT img, safe_title, alt, transcript, date FROM comics WHERE (num = @num)")) // SQL command
                {
                    cmd.Parameters.AddWithValue(@"@num", num); // This is the ID we want to grab
                    
                    conn.Open(); // Open DB connection
                    cmd.Connection = conn;
                    SqlCeDataReader reader = cmd.ExecuteReader(); // Run query
                    try
                    {
                        while (reader.Read())
                        {
                            img = reader.GetString(0); // First item is img
                            safe_title = reader.GetString(1); // Second item is safe_title
                            alt = reader.GetString(2); // Third item is alt
                            transcript = reader.GetString(3); // Fourth item is the transcript
                            date = reader.GetString(4); // Fifth item is the date
                        }
                    }
                    catch
                    {
                        throw; // Isn't working
                    }
                    finally
                    {
                        reader.Close(); // Readers have to be closed
                    }
                }
            }

            Dictionary<string,string> dict = new Dictionary<string,string>();
            dict.Add("img", img);
            dict.Add("safe_title", safe_title);
            dict.Add("alt", alt);
            dict.Add("transcript", transcript);
            dict.Add("date", date);

            return dict;
        }

        public void updateImgData(int num, Image image) // Used by Offline Mode to store actual images
        {
            // Image to Bytes code from http://www.vcskicks.com/image-to-byte.php -- Thanks!
            ImageConverter converter = new ImageConverter();
            byte[] imageBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));

            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source = " + dbPath)) // DB connection
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"UPDATE comics SET img_data = @img_data WHERE num = @num")) // SQL command
                {
                    // Add C# data to SQL query
                    cmd.Parameters.AddWithValue("@num", num);
                    cmd.Parameters.AddWithValue("@img_data", imageBytes);

                    conn.Open(); // Open DB connection
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery(); // Run query
                }
            }
        }

        public Image getImageRow(int num)
        {
            Image img;
            byte[] imgBytes = null;

            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source = " + dbPath)) // DB connection
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"SELECT img_data FROM comics WHERE (num = @num)")) // SQL command
                {
                    cmd.Parameters.AddWithValue(@"@num", num); // This is the ID we want to grab

                    conn.Open(); // Open DB connection
                    cmd.Connection = conn;
                    SqlCeDataReader reader = cmd.ExecuteReader(); // Run query
                    try
                    {
                        while (reader.Read())
                        {
                            imgBytes = (byte[])reader.GetValue(0);
                        }
                    }
                    catch
                    {
                        throw; // Isn't working
                    }
                    finally
                    {
                        reader.Close(); // Readers have to be closed

                        // Translate bytes back into an image
                        ImageConverter converter = new ImageConverter();
                        try
                        {
                            img = (Image)converter.ConvertFrom(imgBytes);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }

            return img;
        }

        public void deleteImgData(int num) // Used to purge Offline Mode images
        {
            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source = " + dbPath)) // DB connection
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"UPDATE comics SET img_data = NULL WHERE num = @num")) // SQL command
                {
                    // Add C# data to SQL query
                    cmd.Parameters.AddWithValue("@num", num);

                    conn.Open(); // Open DB connection
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery(); // Run query
                }
            }
        }
    }
}
