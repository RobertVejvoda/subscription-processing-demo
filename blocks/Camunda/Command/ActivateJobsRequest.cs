using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record ActivateJobsRequest(
        [Required] string JobType,
        [Required] int? MaxJobsToActivate,
        string Timeout,
        string WorkerName,
        IList<string> FetchVariables);
}