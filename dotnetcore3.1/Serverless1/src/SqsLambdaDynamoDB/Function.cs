using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using SqsLambdaDynamoDB.Dtos;
using SqsLambdaDynamoDB.Parallel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace SqsLambdaDynamoDB
{
    public class Function
    {
        private static readonly AmazonDynamoDBClient DynamoDbClient = new AmazonDynamoDBClient(); 
        
        public async Task<string> FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            await sqsEvent.Records?.ForEachAsync(10, async message =>
            {
                var myDto = JsonSerializer.Deserialize<MyDto>(message.Body);
                await SaveToDynamodb(myDto);
            });

            return "done";
        }

        private static async Task SaveToDynamodb(MyDto myDto)
        {
            var request = new UpdateItemRequest
            {
                TableName = "mytable-tes",
                Key = new Dictionary<string,AttributeValue>
                {
                    {
                        "Id", new AttributeValue
                        {
                            S = myDto.Id
                        }
                    }
                },
                UpdateExpression = "SET Fullname = :fullname",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":fullname", new AttributeValue {
                            S =  myDto.Fullname
                        }
                    }
                },
                ReturnValues = "ALL_NEW"
            };
            await DynamoDbClient.UpdateItemAsync(request);
        }
    }
}