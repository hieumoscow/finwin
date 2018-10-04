using System;

namespace Finwin.Backend.Functions
{
	public class ConfigManager
	{
		static ConfigManager _instance;
		public static ConfigManager Instance => _instance ?? (_instance = Load());

        public string NewsApiKey;
        public string BingNewsApiKey;

        public string BingNewsUrl = "https://api.cognitive.microsoft.com/bing/v7.0/news/search?q=category=business";
        public string NewsApiUrl = "https://newsapi.org/v2/everything?q={0}&apiKey={1}";

        public string BlobSharedStorageKey;
        public const string BlobImageNewsContainer = "news";
        public const string BlobImageVideosContainer = "videos";

        public static ConfigManager Load()
		{
            var config = new ConfigManager
            {
                NewsApiKey = "5ca1db9abf5644ae99088b8551cfa221",//Environment.GetEnvironmentVariable("NEWSAPI_KEY"),
                BingNewsApiKey = "674a91c42e56416d87279366e4648e1f",//Environment.GetEnvironmentVariable("BINGNEWS_KEY"),

                BlobSharedStorageKey = "DefaultEndpointsProtocol=https;AccountName=finwinbot9fc3;AccountKey=wpxhNBvn1yYnckJSSr4btZmEoyIy0bp01rB1hF9+gRGwisUf5GfaKWKxcNqiunmESWhqBNQLyaEhJhUQTGmCBQ==;EndpointSuffix=core.windows.net"//Environment.GetEnvironmentVariable("BLOB_SHAREDSTORAGEKEY")
            };


            return config;
		}
	}
}
