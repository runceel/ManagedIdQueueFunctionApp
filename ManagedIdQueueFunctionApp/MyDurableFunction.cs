using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ManagedIdQueueFunctionApp
{
    public static class MyDurableFunction
    {
        [FunctionName("MyDurableFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger logger)
        {
            logger = context.CreateReplaySafeLogger(logger);
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("MyDurableFunction_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("MyDurableFunction_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("MyDurableFunction_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            logger.LogInformation("Outputs: {outputs}, InstanceId: {instanceId}", JsonSerializer.Serialize(outputs), context.InstanceId);
            return outputs;
        }

        [FunctionName("MyDurableFunction_Hello")]
        public static string SayHello([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var name = context.GetInput<string>();
            var r = new Random();
            var l = new List<(double Sin, double Cos)>();
            for(int i = 0; i < 100; i++)
            {
                l.Add(Math.SinCos(r.NextDouble() * 360));
            }

            log.LogInformation("Saying hello to {name}, InstanceId: {instanceId}.", name, context.InstanceId);
            return $"Hello {name}({l.Sum(x => x.Sin + x.Cos)})!";
        }

    }
}