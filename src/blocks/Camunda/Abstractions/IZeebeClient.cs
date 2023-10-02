using Camunda.Command;

namespace Camunda.Abstractions
{
    public interface IZeebeClient
    {
        Task CancelInstanceAsync(CancelInstanceRequest request);
        Task CompleteJobAsync(CompleteJobRequest request);
        Task<CreateInstanceResponse> CreateInstanceAsync(CreateInstanceRequest request);
        Task FailJobAsync(FailJobRequest request);
        Task<IList<ActivatedJob>> ActivateJobsAsync(ActivateJobsRequest request);
        Task<PublishMessageResponse> PublishMessageAsync(PublishMessageRequest request);
        Task ResolveIncidentAsync(ResolveIncident request);
        Task<SetVariablesResponse> SetVariablesAsync(SetVariablesRequest request);
        Task ThrowErrorAsync(ThrowErrorRequest request);
        Task<TopologyResponse> GetTopologyAsync();
        Task UpdateJobRetriesAsync(UpdateJobRetriesRequest request);
    }
}