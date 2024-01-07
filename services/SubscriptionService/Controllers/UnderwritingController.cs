using SubscriptionService.Commands;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/underwriting")]
public class UnderwritingController(SubscriptionRepository repository) : ControllerBase
{
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/request-information")]
    public async Task<ActionResult> RequestInformation(
        [Required] RequestInformationCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.RequestInformation(
            new UnderwritingResult(
                Enum.Parse<UnderwritingResultState>(command.UnderwritingResultState, true), 
                command.UnderwritingResultMessage));

        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        return Ok();
    }

    [HttpPost("/on-information-received")]
    public async Task<ActionResult> InformationReceived(InformationReceivedCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.OnInformationReceived();

        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        return Ok();
    }
}