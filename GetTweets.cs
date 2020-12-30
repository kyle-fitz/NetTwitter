using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using System.Text.RegularExpressions;

namespace NetTwitter
{
    
    public class GetTweets
    {
        const string UserName = "realDonalTrump";
        static void PrintCleanTweetsofHyperLink(List<Status> tweets)
        {

            if (tweets != null)
                tweets.ForEach(tweet =>
                {
                    string pattern = "https://";

                    if (tweet.FullText.Contains(pattern))
                    {

                        string resultTweet = Regex.Replace(tweet.FullText, @"http[^\s]+", "");

                    Console.WriteLine(
                        "This Tweet had a hyperlink that was removed. {0}", resultTweet);
                    }else if (!tweet.FullText.Contains(pattern)){
                        Console.WriteLine(
                            "Tweet with no hyperlink found: {0}", tweet.FullText);    
                    };
                });
        }
        
        //removes the tweets with https -- want to strip the links from the tweet...
        static void PrintTweetsResults(List<Status> tweets)
        {
            if (tweets != null)
                tweets.ForEach(tweet =>
                {
                    if (tweet != null && tweet.User != null && !tweet.Text.Contains("https://"))
                        Console.WriteLine(
                            "Username: {0} && Tweet: {1}", tweet.User.ScreenNameResponse, tweet.Text);
                });
        }

        //Donald Trump tweets to list 
        public static async Task RunUserTimelineQueryAsync(TwitterContext twitterCtx)
        {
            var tweets =
                await
                (from tweet in twitterCtx.Status
                 where tweet.TweetMode == TweetMode.Extended &&
                        tweet.Type == StatusType.User &&
                       tweet.ScreenName == UserName
                 select tweet)
                .ToListAsync();

            PrintCleanTweetsofHyperLink(tweets);
            //PrintTweetsResults(tweets);
        }

        //Search for Donald Trump -- Could be useful for searching for hot words.
        public static async Task NewAsyncReturnSearch(TwitterContext twitterCtx)
        {

            var searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == "\"Donald Trump\""
                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse != null && searchResponse.Statuses != null)
                searchResponse.Statuses.ForEach(tweet =>
                    Console.WriteLine(
                        "User: {0}, Tweet: {1}",
                        tweet.User.ScreenNameResponse,
                        tweet.Text));
        }

        //Donald Trump last Tweet 
        public static async Task ShowUserDetailsAsync(TwitterContext twitterCtx)
        {
            var user =
                await
                (from tweet in twitterCtx.User
                 where tweet.Type == UserType.Show &&
                       tweet.ScreenName == UserName
                 select tweet)
                .SingleOrDefaultAsync();

            if (user != null)
            {
                var name = user.ScreenNameResponse;
                var lastStatus =
                    user.Status == null ? "No Status" : user.Status.Text;

                Console.WriteLine();
                Console.WriteLine(
                    "Name: {0}, Last Tweet: {1}\n", name, lastStatus);
            }
        }

        //Returns raw data searching for unencodedStatus
        public static async Task PerformSearchRawAsync(TwitterContext twitterCtx)
        {
            string unencodedStatus = UserName;
            string encodedStatus = Uri.EscapeDataString(unencodedStatus);
            string queryString = "search/tweets.json?q=" + encodedStatus;

            var rawResult =
                await
                (from raw in twitterCtx.RawQuery
                 where raw.QueryString == queryString
                 select raw)
                .SingleOrDefaultAsync();

            if (rawResult != null)
                Console.WriteLine(
                    "Response from Twitter: \n\n" + rawResult.Response);
        }

        public static async Task RunHomeTimelineQueryAsync(TwitterContext twitterCtx)
        {
            var tweets =
                await
                (from tweet in twitterCtx.Status
                 where tweet.Type == StatusType.Home
                 select tweet)
                .ToListAsync();
                
            PrintTweetsResults(tweets);
        }

        public static async Task GetTwitterJson(TwitterContext twitterCtx)
        {
            string unencodedStatus = UserName;
            string encodedStatus = Uri.EscapeDataString(unencodedStatus);
            string queryString = "search/tweets.json?q=" + encodedStatus;

            var rawResult =
                await
                    (from raw in twitterCtx.RawQuery
                        where raw.QueryString == queryString
                        select raw)
                    .SingleOrDefaultAsync();

            if (rawResult != null)
                Console.WriteLine(rawResult.Response); 
        }  
    }
}
