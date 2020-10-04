using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api
{
    public static class health
    {
        [FunctionName("health")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Blob("familyboard/configuration.json", FileAccess.Write)] out string configuration,
            ILogger log)
        {
            log.LogInformation("Health request.");

            var claim = StaticWebAppsAuth.Parse(req);

            var responseMessage = new
            {
                Status = "Healthy.",
                Claim = claim?.Identity?.Name ?? "none",
            };

            configuration = JsonConvert.SerializeObject(new BoardConfiguration()
            {
                MSAToken = "test",
            }, Formatting.None);

            return new OkObjectResult(responseMessage);
        }
    }
}
