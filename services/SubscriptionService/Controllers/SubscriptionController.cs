namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController(SubscriptionRepository repository) : ControllerBase
{
    private const string BpmnProcessId = "Subscription_Process_Workflow";
    
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
    
    [HttpPost("/register")]
    public async Task<ActionResult<SubscriptionModel>> Register(
        [Required] RegisterSubscriptionCommand command)
    {
        // register a new subscription
        var subscription = new Subscription(command.ProductId, command.LoanAmount, command.InsuredAmount);
        subscription.Register().Normalize();
        await repository.AddAsync(subscription);

        return Ok(subscription.ToModel());
    }
    
    [HttpPost("/validate")]
    public async Task<ActionResult<SubscriptionModel>> Validate(
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
            return Ok(subscription.ToModel());

        // identify current job
        var jobKey = httpContextAccessor.HttpContext?.Request.Headers["X-Zeebe-Job-Key"];
        if (!jobKey.HasValue)
            return BadRequest();

        // throw business error
        var throwErrorRequest = new ThrowErrorRequest(long.Parse(jobKey.Value!), 
            "SUBSCRIPTION_INVALID", validationResult.Reason);
        await zeebeClient.ThrowErrorAsync(throwErrorRequest);

        return Ok(subscription.ToModel());
    }
    
    [HttpPost("/accept")]
    public async Task<ActionResult<SubscriptionModel>> Accept(
        [Required] AcceptSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Accept(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(subscription.ToModel());
    }
    
    [HttpPost("/reject")]
    public async Task<ActionResult<SubscriptionModel>> Reject(
        [Required] RejectSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Reject(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(subscription.ToModel());
    }
    
    [HttpPost("/suspend")]
    public async Task<ActionResult<SubscriptionModel>> Suspend(
        [Required] SuspendSubscriptionCommand command)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Suspend(command.Reason);
        await repository.AddAsync(subscription);

        return Ok(subscription.ToModel());
    }
}