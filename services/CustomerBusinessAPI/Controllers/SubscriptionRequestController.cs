using CustomerBusinessAPI.Model;

namespace CustomerBusinessAPI.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionRequestController(CustomerDataContext dataContext, Queries.Queries queries, IDateTimeProvider dateTimeProvider)
    : ControllerBase
{
    private const string BpmnProcessId = "Subscription_Process_Workflow";
    
    [HttpGet]
    public async Task<ActionResult<ICollection<SubscriptionRequest>>> GetSubscriptionRequests([FromQuery] int take = 15)
    {
        return Ok(await queries.GetSubscriptionRequests(take));
    }

    [HttpGet("{processInstanceKey}")]
    public async Task<ActionResult<SubscriptionRequest>> GetSubscriptionByProcessInstanceKey([Required] string processInstanceKey)
    {
        var result = await queries.FindSubscriptionRequestByProcessInstanceKey(processInstanceKey);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ProcessInstanceKey>> RegisterSubscription(
        [Required] RegisterSubscriptionRequestCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        // trigger processing in Camunda
        var response = await zeebeClient.CreateInstanceAsync(
            new CreateInstanceRequest(BpmnProcessId, null, null, command));

        return Ok(new ProcessInstanceKey(response.ProcessInstanceKey.ToString()));
    }
    
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/register-subscription-request")]
    public async Task<ActionResult> RegisterSubscription(
        [Required] RegisterSubscriptionRequestCommand command,
        [FromServices] IHttpContextAccessor contextAccessor)
    {
        var receivedOn = dateTimeProvider.Now();
        var processInstanceKey = contextAccessor.HttpContext!.Request.Headers["X-Zeebe-Process-Instance-Key"].Single()!;
        
        // save request
        var subscriptionRequest = new SubscriptionRequestEntity
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            BirthDate = command.BirthDate,
            InsuredAmount = command.InsuredAmount,
            LoanAmount = command.LoanAmount,
            LastUpdatedOn = receivedOn,
            ReceivedOn = receivedOn,
            ProductId = command.ProductId,
            ProcessInstanceKey = processInstanceKey
        };
        await dataContext.AddAsync(subscriptionRequest);
        await dataContext.SaveChangesAsync();

        return Ok(new ProcessInstanceKey(processInstanceKey));
    }
    
    [HttpPost("/subscription-registered")]
    public async Task<ActionResult> OnSubscriptionRegistered(
        [Required] SubscriptionRegisteredCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.CustomerId = command.CustomerId;
        subscription.SubscriptionId = command.SubscriptionId;
        subscription.SubscriptionState = command.SubscriptionState;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok(new { subscription.SubscriptionId, subscription.SubscriptionState });
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
        
        return Ok(new { subscription.SubscriptionId, subscription.SubscriptionState });
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
        
        return Ok(new { subscription.SubscriptionId, subscription.SubscriptionState });
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
        
        return Ok(new { subscription.SubscriptionId, subscription.SubscriptionState });
    }
}