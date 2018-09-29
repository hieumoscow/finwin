// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Finwin.Bot.Dialogs
{
    /// <summary>
    /// Demonstrates the following concepts:
    /// - Use a subclass of ComponentDialog to implement a multi-turn conversation
    /// - Use a Waterflow dialog to model multi-turn conversation flow
    /// - Use custom prompts to validate user input
    /// - Store conversation and user state.
    /// </summary>
    /*public class NewsDialog : IDialog<object>
    {
        public NewsDialog(string dialogId) : base(dialogId)
        {
        }

        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext outerDc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            string tempCompany = "not found";
            string tempCompanyItem = "not found";
            string tempNewsType = "not found";
            string tempStockCode = "not found";

            EntityRecommendation entity;
            if (result.TryFindEntity(company, out entity))
            {
                tempCompany = entity.Entity;
            }
            if (result.TryFindEntity(companyItem, out entity))
            {
                tempCompanyItem = entity.Entity;
            }
            if (result.TryFindEntity(newsType, out entity))
            {
                tempNewsType = entity.Entity;
            }
            if (result.TryFindEntity(stockCode, out entity))
            {
                tempStockCode = entity.Entity;
            }

            using (var client = new HttpClient())
            {
                var url = string.Format("http://finwin.azurewebsites.net/api/QueryBingNews");

                var query = new { Query = tempStockCode };
                var content = new StringContent(JsonConvert.SerializeObject(query));

                var response = await client.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                string message = $"response={jsonResponse}";

                await context.PostAsync(message);
            }

            context.Wait(this.MessageReceived);

            return base.BeginDialogAsync(outerDc, options, cancellationToken);
        }

        // Name of  entity
        public const string company = "company"; // fahrenheit";
        public const string companyItem = "company_item"; //celsius";
        public const string newsType = "news_type"; //100";
        public const string stockCode = "stock_code"; //100";

        // methods to handle LUIS intents

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"try 100 f to c";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }*/
}
