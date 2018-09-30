// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Finwin.Bot.Dialogs
{
    /// <summary>
    /// Demonstrates the following concepts:
    /// - Use a subclass of ComponentDialog to implement a multi-turn conversation
    /// - Use a Waterflow dialog to model multi-turn conversation flow
    /// - Use custom prompts to validate user input
    /// - Store conversation and user state.
    /// </summary>
    public class NewsDialog : ComponentDialog
    {
        // Name of  entity
        public const string company = "company"; // fahrenheit";
        public const string companyItem = "company_item"; //celsius";
        public const string newsType = "news_type"; //100";
        public const string stockCode = "stock_code"; //100";

        public IStatePropertyAccessor<NewsState> NewsAccessor { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsDialog"/> class.
        /// </summary>
        /// <param name="botServices">Connected services used in processing.</param>
        /// <param name="botState">The <see cref="UserState"/> for storing properties at user-scope.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> that enables logging and tracing.</param>
        public NewsDialog(IStatePropertyAccessor<NewsState> newsStateAccessor, ILoggerFactory loggerFactory)
            : base(nameof(NewsDialog))
        {
            NewsAccessor = newsStateAccessor ?? throw new ArgumentNullException(nameof(newsStateAccessor));
        }

        /// <summary>
        /// Begin is called to start a new slot dialog.
        /// </summary>
        /// <param name="dialogContext">A handle on the runtime.</param>
        /// <param name="options">This isn't used in this implementation but required for the contract. Potentially it could be used to pass in existing state - already filled slots for example.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A DialogTurnResult indicating the state of this dialog to the caller.</returns>
        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dialogContext, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            // Don't do anything for non-message activities.
            if (dialogContext.Context.Activity.Type != ActivityTypes.Message)
            {
                return await dialogContext.EndDialogAsync(new Dictionary<string, object>());
            }

            // Run prompt
            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        /// <summary>
        /// Continue is called to run an existing dialog. It will return the state of the current dialog. If there is no dialog it will return Empty.
        /// </summary>
        /// <param name="dialogContext">A handle on the runtime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A DialogTurnResult indicating the state of this dialog to the caller.</returns>
        /*public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            // Don't do anything for non-message activities.
            if (dialogContext.Context.Activity.Type != ActivityTypes.Message)
            {
                return EndOfTurn;
            }

            // Run next step with the message text as the result.
            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        /// <summary>
        /// Resume is called when a child dialog completes and we need to carry on processing in this class.
        /// </summary>
        /// <param name="dialogContext">A handle on the runtime.</param>
        /// <param name="reason">The reason we have control back in this dialog.</param>
        /// <param name="result">The result from the child dialog. For example this is the value from a prompt.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A DialogTurnResult indicating the state of this dialog to the caller.</returns>
        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dialogContext, DialogReason reason, object result, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            // Update the state with the result from the child prompt.
            var slotName = (string)dialogContext.ActiveDialog.State[SlotName];
            var values = GetPersistedValues(dialogContext.ActiveDialog);
            values[slotName] = result;

            // Run prompt.
            return await RunPromptAsync(dialogContext, cancellationToken);
        }*/

        /// <summary>
        /// Helper function to deal with the persisted values we collect in this dialog.
        /// </summary>
        /// <param name="dialogInstance">A handle on the runtime instance associated with this dialog, the State is a property.</param>
        /// <returns>A dictionary representing the current state or a new dictionary if we have none.</returns>
        private static IDictionary<string, object> GetPersistedValues(DialogInstance dialogInstance)
        {
            object obj;
            if (!dialogInstance.State.TryGetValue(PersistedValues, out obj))
            {
                obj = new Dictionary<string, object>();
                dialogInstance.State.Add(PersistedValues, obj);
            }

            return (IDictionary<string, object>)obj;
        }

        /// <summary>
        /// This helper function contains the core logic of this dialog. The main idea is to compare the state we have gathered with the
        /// list of slots we have been asked to fill. When we find an empty slot we exectute the corresponding prompt.
        /// </summary>
        /// <param name="dialogContext">A handle on the runtime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A DialogTurnResult indicating the state of this dialog to the caller.</returns>
        private Task<DialogTurnResult> RunPromptAsync(DialogContext dialogContext, CancellationToken cancellationToken)
        {
            var state = GetPersistedValues(dialogContext.ActiveDialog);

            // Run through the list of slots until we find one that hasn't been filled yet.
            var unfilledSlot = _slots.FirstOrDefault((item) => !state.ContainsKey(item.Name));

            // If we have an unfilled slot we will try to fill it
            if (unfilledSlot != null)
            {
                // The name of the slot we will be prompting to fill.
                dialogContext.ActiveDialog.State[SlotName] = unfilledSlot.Name;

                // If the slot contains prompt text create the PromptOptions.

                // Run the child dialog
                return dialogContext.BeginDialogAsync(unfilledSlot.DialogId, unfilledSlot.Options, cancellationToken);
            }
            else
            {
                // No more slots to fill so end the dialog.
                return dialogContext.EndDialogAsync(state);
            }
        }
  
        public async Task SerialNumber(DialogContext context, LuisResult result)
        {
            string tempCompany = "not found";
            string tempCompanyItem = "not found";
            string tempNewsType = "not found";
            string tempStockCode = "not found";

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

                //await context.PostAsync(message);
            }

            //context.Wait(this.MessageReceived);
        }
    }
}