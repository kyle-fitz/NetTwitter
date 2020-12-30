using System;
using LinqToTwitter;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace NetTwitter
{
    internal class Program
    {
        // public static void Main(string[] args)
        static async Task Main(string[] args)
        {
            var tweets = new GetTweets();
            
            var auth = new SingleUserAuthorizer
            {

                CredentialStore = new InMemoryCredentialStore()
                {
                    
                    ConsumerKey = ConfigurationSettings.AppSettings["consumerKey"],
                    ConsumerSecret = ConfigurationSettings.AppSettings["consumerSecret"],
                    OAuthToken = ConfigurationSettings.AppSettings["accessToken"],
                    OAuthTokenSecret = ConfigurationSettings.AppSettings["accessTokenSecret"]
                }

            };
            
            //FileStream > Stream Writer > Check Exists > Delte or Create > Open > Get Console > Write Console > Close
            FileStream fileStream;
            StreamWriter writer;
            TextWriter oldConsole = Console.Out;
            
            //set the filepath to the output of the console.
            var filePath = "";
            var fileName = "TweetList.json";
            var fullPath = filePath + fileName;

            if (File.Exists(Path.Combine(filePath, fileName)))
            {
                File.Delete(Path.Combine(filePath, fileName));
                Console.WriteLine("File Deleted... Preparing output file...");
            }

            try
            {
                fileStream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(fileStream);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot open TweetList.json for writing");
                Console.WriteLine(e.Message);
                return;
            }

            Console.SetOut(writer);

            var twitterCtx = new TwitterContext(auth);
            
            await GetTweets.GetTwitterJson(twitterCtx);
                
            Console.SetOut(oldConsole);

            writer.Close();
            oldConsole.Close();
            
        }
    }
}