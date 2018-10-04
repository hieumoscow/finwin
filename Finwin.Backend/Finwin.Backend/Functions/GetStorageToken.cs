using System.Net;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Finwin.Backend.Functions.Models;

namespace Finwin.Backend.Functions
{
	public static class GetStorageToken
	{
        static CloudStorageAccount _storageAccount;
		static CloudBlobClient _blobClient;
		static CloudBlobContainer _container;

        [FunctionName(nameof(GetStorageToken))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(GetStorageToken))]
                                HttpRequest req)

        {
            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(GetStorageToken)
            }))
            {
                try
                {
                    string blobName = req.Query["blobName"];
                    string containerName = req.Query["containerName"];

                    string requestBody = new StreamReader(req.Body).ReadToEnd();
                    dynamic data = JsonConvert.DeserializeObject(requestBody);

                    blobName = blobName ?? data?.BlobName;
                    containerName = containerName ?? data?.ContainerName;

                    if (string.IsNullOrWhiteSpace(blobName))
                    {
                        var e = new ArgumentNullException(nameof(blobName));

                        // track exceptions that occur
                        analytic.TrackException(e);

                        throw e;
                    }

                    if (_storageAccount == null)
                        _storageAccount = CloudStorageAccount.Parse(ConfigManager.Instance.BlobSharedStorageKey);

                    if (_blobClient == null)
                        _blobClient = _storageAccount.CreateCloudBlobClient();

                    _container = _blobClient.GetContainerReference(containerName);

                    var sasUri = await GetSASToken(blobName);

                    return (ActionResult)new OkObjectResult(sasUri);
                }
                catch (Exception e)
                {
                    // track exceptions that occur
                    analytic.TrackException(e);

                    return new BadRequestObjectResult(e);
                }
            }
        }

        /// <summary>
        /// Returns SAS token for a particular blob
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static async Task<string> GetSASToken(string blobName, string policyName = null)
		{
			string sasBlobToken = string.Empty;
			var blob = _container.GetBlockBlobReference(blobName);

			if (policyName == null)
			{
				SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
				{
					SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
					Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
				};

				sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);
			}
			else
			{
				sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
			}

			return blob.Uri + sasBlobToken;
		}
	}
}