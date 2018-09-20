using System;
using System.Net.Http;
using System.Threading.Tasks;

using Finwin.Backend.Functions;

namespace Finwin.Backend.Services
{
    /// <summary>
    /// News API service.
    /// </summary>
    public class NewsApiService : IDisposable
    {
        /// <summary>
        /// Query this instance.
        /// </summary>
        /// <returns>The query.</returns>
        public async Task<object> Query(string query)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(ConfigManager.Instance.NewsApiUrl, query);

                var response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
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
