using System.ComponentModel.DataAnnotations;

namespace Camunda.Command;

public record CreateInstanceWithResultResponse(
    [Required] long ProcessDefinitionKey,
    [Required] string BpmnProcessId,
    [Required] int Version,
    [Required] long ProcessInstanceKey,
    string? Variables);