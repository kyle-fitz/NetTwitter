using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace NetTwitter
{
    
    public class GetTweets
    { 
        const string SearchableString = "17995040";
        // const string SearchableString = "benshapiro";
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
        
        public static async Task RunUserTimelineQueryAsync(TwitterContext twitterCtx)
        {
            var tweets =
                await
                (from tweet in twitterCtx.Status
                 where tweet.TweetMode == TweetMode.Extended &&
                        tweet.Type == StatusType.User &&
                       tweet.ScreenName == SearchableString
                 select tweet)
                .ToListAsync();

            PrintCleanTweetsofHyperLink(tweets);
            //PrintTweetsResults(tweets);
        }
        
        public static async Task NewAsyncReturnSearch(TwitterContext twitterCtx)
        {

            var searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == "\"Ben Shapiro\""
                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse != null && searchResponse.Statuses != null)
                searchResponse.Statuses.ForEach(tweet =>
                    Console.WriteLine(
                        "User: {0}, Tweet: {1}",
                        tweet.User.ScreenNameResponse,
                        tweet.Text));
        }
        
        public static async Task ShowUserDetailsAsync(TwitterContext twitterCtx)
        {
            var user =
                await
                (from tweet in twitterCtx.User
                 where tweet.Type == UserType.Show &&
                       tweet.ScreenName == SearchableString
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

        //Returns raw json 
        public static async Task PerformSearchRawAsync(TwitterContext twitterCtx)
        {
            string unencodedStatus = SearchableString;
            string encodedStatus = Uri.EscapeDataString(unencodedStatus);
            string queryString = "search/tweets.json?q=" + encodedStatus;
            
            var rawResult =
                await
                (from raw in twitterCtx.RawQuery
                 where raw.QueryString == queryString
                 select raw)
                .SingleOrDefaultAsync();

            if (rawResult != null)
                Console.WriteLine(JToken.Parse(rawResult.Response).ToString(Newtonsoft.Json.Formatting.Indented));
        }
        
        public static async Task PerformRecentSearchRawAsync(TwitterContext twitterCtx)
        {
            _ = twitterCtx ?? throw new ArgumentNullException(nameof(twitterCtx));

            string unencodedStatus = SearchableString;
            string encodedStatus = Uri.EscapeDataString(unencodedStatus);
            string queryString = "users/" + encodedStatus +
                                 "/tweets?tweet.fields=created_at&expansions=author_id&user.fields=created_at&exclude=retweets&max_results=100";
            
            string previousBaseUrl = twitterCtx.BaseUrl;
            twitterCtx.BaseUrl = "https://api.twitter.com/2/";

            var rawResult =
                await
                    (from raw in twitterCtx.RawQuery
                        where raw.QueryString == queryString
                        select raw)
                    .SingleOrDefaultAsync();

            if (rawResult != null)
                Console.WriteLine(
                    JToken.Parse(rawResult.Response).ToString(Newtonsoft.Json.Formatting.Indented));

            twitterCtx.BaseUrl = previousBaseUrl;
        }
        
        public static async Task PerformUserStatusRawAsync(TwitterContext twitterCtx)
        {
            _ = twitterCtx ?? throw new ArgumentNullException(nameof(twitterCtx));

            string unencodedStatus = SearchableString;
            string encodedStatus = Uri.EscapeDataString(unencodedStatus);
            string queryString = "statuses/user_timeline.json?screen_name=" + encodedStatus;
            
            string previousBaseUrl = twitterCtx.BaseUrl;
            twitterCtx.BaseUrl = "https://api.twitter.com/1.1/";

            var rawResult =
                await
                    (from raw in twitterCtx.RawQuery
                        where raw.QueryString == queryString
                        select raw)
                    .SingleOrDefaultAsync();

            if (rawResult != null)
                Console.WriteLine(
                    JToken.Parse(rawResult.Response).ToString(Newtonsoft.Json.Formatting.Indented));

            twitterCtx.BaseUrl = previousBaseUrl;
        }
        
    }
}
