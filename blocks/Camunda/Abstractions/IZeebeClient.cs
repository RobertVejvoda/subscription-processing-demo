using Camunda.Command;

namespace Camunda.Abstractions
{
    public interface IZeebeClient
    {
        Task<CreateInstanceResponse> CreateInstanceAsync(CreateInstanceRequest request);
        Task<CreateInstanceResponse> CreateInstanceWithResultAsync(CreateInstanceWithResultRequest request);
        Task<PublishMessageResponse> PublishMessageAsync(PublishMessageRequest request);
        Task<SetVariablesResponse> SetVariablesAsync(SetVariablesRequest request);
        Task ThrowErrorAsync(ThrowErrorRequest request);
        Task UpdateJobRetriesAsync(UpdateJobRetriesRequest request);
    }
}