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

        public static ConfigManager Load()
		{
			var config = new ConfigManager
			{
                NewsApiKey = Environment.GetEnvironmentVariable("NEWSAPI_KEY"),
                BingNewsApiKey = Environment.GetEnvironmentVariable("BINGNEWS_KEY"),
            };

			return config;
		}
	}
}
