using System.Net;
using System.Net.Http;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ApplicationInsights.DataContracts;

using Newtonsoft.Json;
using Finwin.Backend.Services;

namespace Finwin.Backend.Functions
{
    public static class QueryBingNews
    {
        [FunctionName(nameof(QueryBingNews))]

        async public static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = nameof(QueryBingNews))]
            HttpRequestMessage req, TraceWriter log)
        {
            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(QueryBingNews)
            }))
            {
                try
                {
                    var query = "";//req.GetQueryNameValuePairs().FirstOrDefault(kvp => kvp.Key == "query").Value;

                    using (var newsApi = new BingNewsService())
                    {
                        var news = await newsApi.Query(query);

                        return req.CreateResponse(HttpStatusCode.OK, news);
                    }
                }
                catch (Exception e)
                {
                    analytic.TrackException(e);

                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                }
            }
        }
    }
}