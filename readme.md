# Processing Subscription demo

Lets assume that we want to process subscriptions (subscription becomes policy) and split processing into mulitple microservices. 
There are possibly many ways how to achieve the goal. A typical approach is use some message oriented middleware like Kafka or RabbitMQ
and exchange messages. While this is perfectly acceptable the other approach is to use some workflow orchestrator, in our case Camunda Platform 8.
The BPMN below describes the flow.

![image](/Users/robert/source/repos/cardif/subscription-processing-demo/assets/subscription-processing.png)

The first task is to register a customer in CRM system or - in our case - Customer Service.
Then Subscription Service is responsible for registering and validating the request.
Assessment of incoming subscription can be implemented in DMN tables or in a more sophisticated way 
using custom Underwriting Service. After acceptance the subscription is sent to PMS (policy management system) 
for further processing and activation. This part is outside of this scope.

## Architecture

For high availability systems I've decided to split commands and queries (CQRS pattern). 
Each microservice has its own data store, but to provide the full visibility on customer we need to build simple ODS 
(operational data store) which will be responsible for aggregating all customer portfolio and display a list of policies 
or status of raised claims in future.

Camunda orchestrates the flow. 
ODS is using PubSub pattern with RabbitMQ to build customer views. 

![image](assets/target_architecture.png)

## Subscription Model

Subscription model is implemented using domain driven design. 

![image](assets/subscription_states.png)

## Subscription validation - underwriting process

Given the age and insured amount decide risk and make a final decision whether to accept subscription request.

![image](assets/register-customer.png)

![image](assets/underwriting.png)

![image](assets/underwriting_risk.jpg)

---

## Setup

| Service                   | Application Port | Dapr sidecar HTTP port | Dapr sidecar gRPC port | Metrics port |
|---------------------------|------------------|------------------------|------------------------|--------------|
| SubscriptionService       | 5001             | 3601                   | 60001                  | 9091         |
| CustomerService           | 5002             | 3602                   | 60002                  | 9092         |
| UnderwritingService [tbd] | 5003             | 3603                   | 60003                  | 9093         |
| PartnerService [tbd]      | 5004             | 3604                   | 60004                  | 9094         |
| ProductService [tbd]      | 5005             | 3605                   | 60005                  | 9095         |
| ODSService                | 5010             | 3610                   | 60010                  | 9100         | 

---


### Builds

I'm using Docker buildx by default for multi-platform builds to support both platforms - linux/amd64 and linux/arm64.

```terminal
docker buildx build --pull --push -t localhost:5000/customer-service -f ./CustomerService/Dockerfile --platform linux/arm64,linux/arm,linux/amd64 .
docker buildx build --pull --push -t localhost:5000/subscription-service -f ./SubscriptionService/Dockerfile --platform linux/arm64,linux/arm,linux/amd64 .
```

### Run & test

`docker compose -f docker-compose.yaml up -d --build`

File requests.http contains REST client scripts and is perhaps better.

```
POST http://localhost:5001/api/subscriptions
dapr-app-id: subscription-service
content-type: application/json

{
  "firstName": "Homer",
  "lastName": "Simpson",
  "email": "homer.simpson@thesimpsons.movie",
  "birthDate": "01-01-2000",
  "productId": "1",
  "loanAmount": 200000,
  "insuredAmount": 100000
}
```

and check subscription state:

```
GET http://localhost:5001/api/subscriptions/{subscriptionId}
```

### Notes

Camunda Platform 8 supports many connectors our of the box, but I prefer to leave the work on microservice 
where Dapr does a great job to subscribe events. 