using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ManagedIdQueueFunctionApp
{
    public class QueueFunction
    {
        [FunctionName("QueueFunction")]
        public async Task Run([QueueTrigger("input", Connection = "QueueInput")] string myQueueItem,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            if (int.TryParse(myQueueItem, out var count))
            {
                await Task.WhenAll(Enumerable.Range(0, count)
                    .Select(x => starter.StartNewAsync("MyDurableFunction")));
            }
        }
    }
}
