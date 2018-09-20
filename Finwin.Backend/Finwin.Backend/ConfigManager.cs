using System;

namespace Finwin.Backend.Functions
{
	public class ConfigManager
	{
		static ConfigManager _instance;
		public static ConfigManager Instance => _instance ?? (_instance = Load());
		public static string NewsApiKey = "5ca1db9abf5644ae99088b8551cfa221";
        public string NewsApiUrl;

		public static ConfigManager Load()
		{
			var config = new ConfigManager
			{
                NewsApiUrl = "https://newsapi.org/v2/everything?q={0}&apiKey=" + NewsApiKey
            };

			return config;
		}
	}
}
