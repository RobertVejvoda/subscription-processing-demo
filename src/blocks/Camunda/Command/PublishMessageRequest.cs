using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record PublishMessageRequest(
        [Required] string MessageName,
        string CorrelationKey,
        string MessageId,
        string TimeToLive,
        Dictionary<string, string> Variables);
}