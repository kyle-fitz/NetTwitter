using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NetTwitter.Classes;
using Newtonsoft.Json;

namespace NetTwitter
{
    public class ParseTweets
    {
        string AppFilePath = ConfigurationSettings.AppSettings["TextFilePath"];
        List<RawTweet> lstRawTweets = new List<RawTweet>();
        
        public void LoadJson()
        {
            using (StreamReader r = new StreamReader(AppFilePath))
            {
                string json = r.ReadToEnd();
                lstRawTweets = JsonConvert.DeserializeObject<List<RawTweet>>(json);

                foreach (RawTweet item in lstRawTweets)
                {
                    Console.WriteLine(item.ToString() + "\n");
                }
        
            }
            
            
        }
    }
}