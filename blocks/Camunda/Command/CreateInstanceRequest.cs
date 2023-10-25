using System.Collections.Generic;

namespace Camunda.Command
{
    public record CreateInstanceRequest(
        string BpmnProcessId,
        long? ProcessDefinitionKey,
        int? Version,
        Dictionary<string, string> Variables);
}