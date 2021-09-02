using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsLambdaDynamoDB.Dtos;
using SqsLambdaDynamoDB.Parallel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace PushToSqs
{
    public class Function
    {
        private static readonly AmazonSQSClient AmazonSqsClient = new AmazonSQSClient(); 
        public async Task<string> FunctionHandler(ILambdaContext context)
        {
            var chunks = Enumerable.Range(1, 100000)
                .Select((value, index) => new { Value = value, Index = index })
                .GroupBy(i => i.Index / 10, v => v.Value);
            await chunks.ForEachAsync(10, async chunk =>
            {
                var batchRequest = new SendMessageBatchRequest
                {
                    QueueUrl = "https://sqs.eu-central-1.amazonaws.com/xxxxx/my-sqs"
                };
                foreach (var index in chunk.ToList())
                {
                    batchRequest.Entries.Add(new SendMessageBatchRequestEntry
                    {
                        MessageBody = JsonSerializer.Serialize(new MyDto
                        {
                            Id = $"{index}{DateTime.Now.ToFileTime()}",
                            Fullname = Guid.NewGuid().ToString()
                        }),
                        Id = Guid.NewGuid().ToString()
                    });
                }
                
                await AmazonSqsClient.SendMessageBatchAsync(batchRequest);

            });

            return "done";
        }

    }
}