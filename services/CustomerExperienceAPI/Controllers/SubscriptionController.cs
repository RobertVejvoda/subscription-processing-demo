namespace CustomerExperienceAPI.Controllers;

[ApiController]
[Route("")]
public class SubscriptionController(
    CustomerDataContext dataContext,
    IDateTimeProvider dateTimeProvider)
    : ControllerBase
{
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/subscription-registered")]
    public async Task<ActionResult> OnSubscriptionRegistered(
        [Required] SubscriptionRegisteredCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionId = command.SubscriptionId;
        subscription.SubscriptionState = command.SubscriptionState;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok(new { subscription.SubscriptionId, subscription.LastUpdatedOn });
    }
    
    [HttpPost("/subscription-accepted")]
    public async Task<ActionResult> AcceptSubscription(
        [Required] SubscriptionAcceptedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok(new { subscription.SubscriptionId, subscription.LastUpdatedOn });
    }

    [HttpPost("/subscription-rejected")]
    public async Task<ActionResult> OnSubscriptionRejected(
        [Required] SubscriptionRejectedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok(new { subscription.SubscriptionId, subscription.LastUpdatedOn });
    }
    
    [HttpPost("/subscription-suspended")]
    public async Task<ActionResult> OnSubscriptionSuspended(
        [Required] SubscriptionSuspendedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok(new { subscription.SubscriptionId, subscription.LastUpdatedOn });
    }
 }