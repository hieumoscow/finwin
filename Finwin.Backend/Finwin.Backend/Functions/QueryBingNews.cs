﻿
using System;
using System.Threading.Tasks;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.ApplicationInsights.DataContracts;

using Newtonsoft.Json;

using Finwin.Backend.Services;

namespace Finwin.Backend.Functions
{
    public static class QueryBingNews
    {
        [FunctionName(nameof(QueryBingNews))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = nameof(QueryBingNews))]
                                        HttpRequest req)
        {
            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(QueryBingNews)
            }))
            {
                try
                {
                    string query = req.Query["query"];

                    string requestBody = new StreamReader(req.Body).ReadToEnd();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);
                    query = query ?? data?.Query;

                    using (var newsApi = new BingNewsService())
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