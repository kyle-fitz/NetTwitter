using System.Collections.Generic;

namespace NetTwitter.Classes
{
    public class RawTweet
    {
        public string type { get; set; }

        public string text { get; set; }
        
        public class RootRawTweet
        {
            public List<RawTweet> data { get; set; }
        }
    }
}