namespace SubscriptionService.Commands;

public record InformationReceivedCommand(
    [Required] string SubscriptionId, 
    [Required] string ProcessInstanceKey);