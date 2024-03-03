namespace SubscriptionService.Dto;

public class UnderwritingRequest(
    [Required] string requestId,
    [Required] string customerId,
    [Required] int age,
    [Required] decimal insuredAmount,
    [Required] string underwritingResultState,
    string underwritingResultMessage)
{
    public string RequestId { get; init; } = requestId;
    public string CustomerId { get; init; } = customerId;
    public int Age { get; init; } = age;
    public decimal InsuredAmount { get; init; } = insuredAmount;
    public string UnderwritingResultState { get; set; } = underwritingResultState;
    public string UnderwritingResultMessage { get; set; } = underwritingResultMessage;
}
    