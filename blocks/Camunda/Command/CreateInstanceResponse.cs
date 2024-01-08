using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record CreateInstanceResponse(
        [Required] long ProcessDefinitionKey,
        [Required] string BpmnProcessId,
        [Required] int Version,
        [Required] long ProcessInstanceKey);
}