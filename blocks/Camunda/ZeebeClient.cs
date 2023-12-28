using Camunda.Abstractions;
using Camunda.Command;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace Camunda
{
    public class ZeebeClient : IZeebeClient
    {
        private const string ZeebeCommand = "zeebe-command";
        private readonly DaprClient daprClient;
        private readonly ILogger logger;

        public ZeebeClient(DaprClient daprClient, ILogger<ZeebeClient> logger)
        {
            this.daprClient = daprClient;
            this.logger = logger;
        }

        public async Task<CreateInstanceResponse> CreateInstanceAsync(CreateInstanceRequest request)
        {
            if (request.BpmnProcessId == null && request.ProcessDefinitionKey == null)
            {
                throw new ArgumentException("Set either bpmnProcessId or processDefinitionKey.");
            }

            return await daprClient.InvokeBindingAsync<CreateInstanceRequest, CreateInstanceResponse>(ZeebeCommand,
                Commands.CreateInstance, request);
        }
        
        public async Task<CreateInstanceResponse> CreateInstanceWithResultAsync(CreateInstanceWithResultRequest request)
        {
            if (request.BpmnProcessId == null)
            {
                throw new ArgumentException("Set bpmnProcessId.");
            }

            return await daprClient.InvokeBindingAsync<CreateInstanceWithResultRequest, CreateInstanceResponse>(ZeebeCommand,
                Commands.CreateInstance, request);
        }


        public Task<SetVariablesResponse> SetVariablesAsync(SetVariablesRequest request)
            => daprClient.InvokeBindingAsync<SetVariablesRequest, SetVariablesResponse>(ZeebeCommand,
                Commands.SetVariables, request);

        public Task<PublishMessageResponse> PublishMessageAsync(PublishMessageRequest request)
            => daprClient.InvokeBindingAsync<PublishMessageRequest, PublishMessageResponse>(ZeebeCommand,
                Commands.PublishMessage, request);

        public Task UpdateJobRetriesAsync(UpdateJobRetriesRequest request)
            => daprClient.InvokeBindingAsync(ZeebeCommand, Commands.UpdateJobRetries, request);

        public Task ThrowErrorAsync(ThrowErrorRequest request)
            => daprClient.InvokeBindingAsync(ZeebeCommand, Commands.ThrowError, request);

    }
}