[![Docker Image CI](https://github.com/RobertVejvoda/dapr-zeebe-demo/actions/workflows/docker-image.yml/badge.svg)](https://github.com/RobertVejvoda/dapr-zeebe-demo/actions/workflows/docker-image.yml)

# Processing Subscription demo

Lets process and validate incoming subscriptions by exposing microservice APIs and bind them events.

![image](Assets/subscription-workflow.png)

## Architecture

![image](Assets/target_architecture.png)

## Subscription Model

![image](Assets/subscription_states.png)

## Subscription validation - underwriting process

![image](Assets/underwriting.png)

![image](Assets/underwriting_risk.jpg)

---

## Run

### Self hosted

| Service             | Application Port | Dapr sidecar HTTP port | Dapr sidecar gRPC port | Metrics port |
|---------------------|------------------|------------------------|------------------------|--------------|
| SubscriptionService | 5001             | 3601                   | 60001                  | 9090         |
| ClientService       | 5002             | 3602                   | 60002                  | 9090         |
| UnderwritingService | 5003             | 3603                   | 60003                  | 9090         |
| PartnerService      | 5004             | 3604                   | 60004                  | 9090         |
| ProductService      | 5005             | 3605                   | 60005                  | 9090         |

Install Dapr and initialize in local environment: https://docs.dapr.io/getting-started/install-dapr-selfhost/

`dapr init`

Expected output:

- dapr_placement container is running.
- dapr_redis container is running.
- dapr_zipkin container is running.

If the placement service is not running, it can't bind zeebe and other components. This can happen if initialized
via `dapr init -slim` command.

Run dapr dashboard (optional) on port 8090, because Camunda is already occupying default port
8080: `dapr dashboard -p 8090`

If port is already allocated, find process and kill:

```
lsof -i tcp:5056
kill 2309
```

In fact, only the placement service is needed here, so Dapr can be initialized as `dapt init --slim`.

---

### Docker

Ensure worker files in dapr/components folder use host.docker.internal instead of localhost.

Run app: `docker compose -f docker-compose.yaml up --build`

### Kubernetes

[todo]

### Tests

Depends on how it's run, change host.docker.internal to localhost in dapr/components folder and run in
terminal: `dotnet run`. Dapr is automatically attached to the process.

File requests.http contains REST client scripts and is perhaps better. I added a process version for each script as I
believe it's a good rule.

Run app first: `dotnet run`

Run in another terminal - register client via invoking zeebe-command in Rest-Client:

```
### 

POST http://localhost:3601/v1.0/bindings/pubsub
dapr-app-id: subscription-api
content-type: application/json

{ 
  "operation": "create-instance", 
  "data": {
    "bpmnProcessId": "Subscription_Process_Workflow", 
    "version": 1, 
    "variables": 
    {
      "firstName": "Homer",
      "lastName": "Simpson",
      "email": "homer.simpson@thesimpsons.movie",
      "age": 30,
      "loanAmount": 200000,
      "insuredAmount": 100000
    } 
  } 
}
```

### Notes

Controller method MUST return a value. Returning void leads to JSON parse issue. It can be solved by returning empty
object like NullResponse.

Content type MUST be specified in header: `curl -H "Content-Type: application-json" ...`

For running locally dapr-app-id must be specified in header: `curl -H "dapr-app-id: client-api" ...`

Return type of the message is passed back to global scope of Camunda variables:

```terminal
result | "Client xxx registered."
```

### Builds and Deployments

I'm using Docker buildx by default for multi-platform builds to support both platforms - linux/amd64 and linux/arm64.

```
docker buildx build --pull --push -t docker.io/robertvejvoda/client-api -f ./ClientAPI/Dockerfile --platform linux/arm64,linux/arm,linux/amd64 .
docker buildx build --pull --push -t docker.io/robertvejvoda/subscription-api -f ./SubscriptionAPI/Dockerfile --platform linux/arm64,linux/arm,linux/amd64 .
```

```
docker compose up --build -d
```

