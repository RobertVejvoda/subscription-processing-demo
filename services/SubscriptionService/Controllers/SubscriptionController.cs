using Microsoft.OpenApi.Extensions;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController(SubscriptionRepository repository) : ControllerBase
{
    [HttpGet("{subscriptionId}")]
    public async Task<ActionResult<SubscriptionModel>> GetSubscription(string subscriptionId)
    {
        Guard.ArgumentNotNullOrEmpty(subscriptionId);

        var subscription = await repository.GetAsync(subscriptionId);
        if (subscription == null)
            return NotFound();

        return Ok(subscription.ToModel());
    }
    
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/register-subscription")]
    public async Task<ActionResult> Register(
        [Required] RegisterSubscriptionCommand command,
        [Required] IHttpContextAccessor contextAccessor)
    {
        var processInstanceKey = contextAccessor.HttpContext!.Request.Headers["X-Zeebe-Process-Instance-Key"];
        
        // register a new subscription
        var subscription = new Subscription(command.ProductId, command.LoanAmount, command.InsuredAmount);
        subscription.Register().Normalize();
        await repository.AddAsync(subscription);

        return Ok(new { subscription.SubscriptionId, SubscriptionState = subscription.State.GetDisplayName(), 
            ProcessInstanceKey = processInstanceKey.Single() });
    }
    
    [HttpPost("/validate-subscription")]
    public async Task<ActionResult> Validate(
        [Required] ValidateSubscriptionCommand command,
        [FromServices] IZeebeClient zeebeClient,
        [FromServices] ProductProxyService productProxyService,
        [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return BadRequest();

        subscription.CustomerId = command.CustomerId;
        var validationResult = subscription.Validate(productProxyService);
        await repository.AddAsync(subscription);
        
        if (validationResult.IsValid) 
            return Ok(new { subscription.SubscriptionId, SubscriptionState = subscription.State.GetDisplayName() });

        // identify current job
        var jobKey = httpContextAccessor.HttpContext?.Request.Headers["X-Zeebe-Job-Key"];
        if (!jobKey.HasValue)
            return BadRequest();

        // throw business error
        var throwErrorRequest = new ThrowErrorRequest(long.Parse(jobKey.Value!), 
            "SUBSCRIPTION_INVALID", validationResult.Reason);
        await zeebeClient.ThrowErrorAsync(throwErrorRequest);

        return Ok(
            new { 
                subscription.SubscriptionId, 
                SubscriptionState = subscription.State.GetDisplayName(),
                validationResult.Reason
        });
    }
    
    [HttpPost("/accept-subscription")]
    public async Task<ActionResult> Accept(
        [Required] AcceptSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Accept(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(new { subscription.SubscriptionId, SubscriptionState = subscription.State.GetDisplayName() });
    }
    
    [HttpPost("/reject-subscription")]
    public async Task<ActionResult> Reject(
        [Required] RejectSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Reject(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(new { subscription.SubscriptionId, SubscriptionState = subscription.State.GetDisplayName() });
    }
    
    [HttpPost("/suspend-subscription")]
    public async Task<ActionResult> Suspend(
        [Required] SuspendSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Suspend(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(new { subscription.SubscriptionId, SubscriptionState = subscription.State.GetDisplayName() });
    }
}