using System;
using System.Configuration;
using System.IO;
using LinqToTwitter;
using Newtonsoft.Json;

namespace NetTwitter
{
    public class ParseTweets
    {
        string AppFilePath = ConfigurationSettings.AppSettings["filePath"];
        
        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(AppFilePath))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                foreach (var item in array)
                {
                    Console.WriteLine();
                }
        
            }
            
            
        }
    }
}