using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Finwin.Backend.Functions;
using Finwin.Backend.Contracts;

namespace Finwin.Backend.Services
{
    /// <summary>
    /// News API service.
    /// </summary>
    public class BingNewsService : IDisposable
    {
        /// <summary>
        /// Query this instance.
        /// </summary>
        /// <returns>The query.</returns>
        public async Task<BingNewsContract> Query(string query)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ConfigManager.Instance.BingNewsUrl, query);

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigManager.Instance.BingNewsApiKey);

                var response = await client.GetAsync(url);

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BingNewsContract>(json);
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Finwin.Backend.Services.NewsApiService"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Finwin.Backend.Services.NewsApiService"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Finwin.Backend.Services.NewsApiService"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Finwin.Backend.Services.NewsApiService"/> so the garbage collector can reclaim the memory that
        /// the <see cref="T:Finwin.Backend.Services.NewsApiService"/> was occupying.</remarks>
        public void Dispose()
        {
        }
    }
}
