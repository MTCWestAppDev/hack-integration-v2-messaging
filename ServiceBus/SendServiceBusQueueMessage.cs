using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServiceBus
{
    public static class SendServiceBusQueueMessage
    {
        [FunctionName("SendServiceBusQueueMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [ServiceBus("sbq-test", Connection = "ServiceBusConnection")] IAsyncCollector<dynamic> outputServiceBus,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string message = data?.message;

            log.LogInformation("message value: " + message);

            await outputServiceBus.AddAsync(message);

            return new OkObjectResult(null);
        }
    }
}
