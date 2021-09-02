/// <reference types="node" />
import { SQSEvent, SQSRecord } from "aws-lambda";
import DynamoDB from "aws-sdk/clients/dynamodb";

const dynamoDbClient = new DynamoDB.DocumentClient();

export const handler = async (event: SQSEvent, context: any): Promise<any> => {
  console.log(JSON.stringify(event));
  await Promise.all(event.Records.map(async (message: SQSRecord) => {
    console.log(JSON.stringify(message));
    const myDto: MyDto = JSON.parse(message.body);
    await saveToDynamodb(myDto);
  }));

  return "done";
};

async function saveToDynamodb(myDto: MyDto): Promise<void> {
  const params = {
    TableName: "mytable-test",
    Key: {
      "Id": myDto.Id
    },
    UpdateExpression: "SET Fullname = :fullname",
    ExpressionAttributeValues: {
      ":fullname": myDto.Fullname
    },
    ReturnValues: "UPDATED_NEW"
  };

  await dynamoDbClient.update(params).promise();
}

export interface MyDto {
  Id: string;
  Fullname: string;
}
