using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record UpdateJobRetriesRequest(
        [Required] long? JobKey,
        int? Retries);
}