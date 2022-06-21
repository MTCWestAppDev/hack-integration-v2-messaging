using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;

namespace ServiceBus
{
    public static class SendServiceBusTopicMessage
    {
        [FunctionName("SendServiceBusTopicMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [ServiceBus("sbt-test" ,Connection = "ServiceBusConnection")] IAsyncCollector<dynamic> outputServiceBus,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string message = data?.message;

            log.LogInformation("message value: " + requestBody);

            await outputServiceBus.AddAsync(requestBody);

            return new OkObjectResult(null);
        }
    }
}
