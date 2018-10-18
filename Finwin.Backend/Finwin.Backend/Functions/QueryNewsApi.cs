using System.Net;
using System.Net.Http;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs;
using Microsoft.ApplicationInsights.DataContracts;

using Newtonsoft.Json;

using Finwin.Backend.Services;

namespace Finwin.Backend.Functions
{
    public static class QueryNewsApi
    {
        [FunctionName(nameof(QueryNewsApi))]
        public static async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = nameof(QueryNewsApi))]
                                        HttpRequest req, TraceWriter log)
        {
            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(QueryNewsApi)
            }))
            {
                try
                {
                    string query = req.Query["query"];

                    string requestBody = new StreamReader(req.Body).ReadToEnd();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);
                    query = query ?? data?.Query;

                    using (var newsApi = new NewsApiService())
                    {
                        var news = await newsApi.Query(query);
                        return new OkObjectResult(news);
                    }
                }
                catch (Exception e)
                {
                    analytic.TrackException(e);
                    return new BadRequestObjectResult(e);
                }
            }
        }
    }
}