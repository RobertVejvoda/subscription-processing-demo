namespace Camunda.Command
{
    public record CreateInstanceRequest(
        string BpmnProcessId,
        long? ProcessDefinitionKey,
        int? Version,
        object Variables);
}