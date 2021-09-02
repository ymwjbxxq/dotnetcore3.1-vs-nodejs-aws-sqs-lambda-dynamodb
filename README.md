# dotnetcore3.1 vs node.js14.x

This is my second series of comparisons. The [first](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-stepfunction-test) was a more cold start test, while on this one, I want to check how they compare in a warm status.

### What I did ###

Two Lambda functions:

* [Dotnetcore3.1](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/tree/main/dotnetcore3.1/Serverless1/src/SqsLambdaDynamoDB)
* [Nodejs14.x](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/tree/main/node14.x/SqsLambdaDynamoDB)

They get the SQS event as input and save it into DynamoDB.

### TESTS ###

The first noticeable thing is the package size:

* Process 200K SQS messages
* Process 400K SQS messages

### METRICS TEST 200K SQS ###

**Nodejs14.x:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/node1.1.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/node1.2.png)

**Dotnetcore3.1:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/dotnet1.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/dotnet1.2.png)

**Insight:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/insight1.png)

### METRICS TEST 400K SQS ###

**Nodejs14.x:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/node2.1.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/node2.2.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/node2.3.png)

**Dotnetcore3.1:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/dotnet2.1.png)

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/dotnet2.2.png)

**Insight:**

![picture](https://github.com/ymwjbxxq/dotnetcore3.1-vs-nodejs-aws-sqs-lambda-dynamodb/blob/main/insight2.png)

### Conclusion ###

Dotnetcore3.1 is the winner, and even if it could have a slower start, it is much faster in a warm serverless scenario.

Faster means a cheap bill, and with 1ms pricing, there is now more of an incentive to optimize the duration of functions.
