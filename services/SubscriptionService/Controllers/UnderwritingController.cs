using SubscriptionService.Commands;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/underwriting")]
public class UnderwritingController : ControllerBase
{
    private readonly SubscriptionRepository repository;

    public UnderwritingController(SubscriptionRepository repository)
    {
        this.repository = repository;
    }
    
    [HttpPost("/request-information")]
    public async Task<ActionResult> RequestInformation(
        [Required] RequestInformationCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.RequestInformation(command.UnderwritingResult);

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