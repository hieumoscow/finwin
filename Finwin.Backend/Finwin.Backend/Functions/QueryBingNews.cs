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
using Finwin.Backend.Contracts;

namespace Finwin.Backend.Functions
{
    public static class QueryBingNews
    {
        [FunctionName(nameof(QueryBingNews))]

        async public static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(QueryBingNews))]
            HttpRequestMessage req)
        {
            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(QueryBingNews)
            }))
            {
                try
                {
                    var json = await req.Content.ReadAsStringAsync();
                    var query = JsonConvert.DeserializeObject<QueryContract>(json);

                    using (var newsApi = new BingNewsService())
                    {
                        var news = await newsApi.Query(query.Query);

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