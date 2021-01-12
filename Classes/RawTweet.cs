namespace NetTwitter.Classes
{
    public class RawTweet
    {
        private string _type;

        public string Type
        {
            get => _type;
            set => _type = value;
        }

        private string _text;

        public string Text
        {
            get => _text;
            set => _text = value;
        }
    }
}