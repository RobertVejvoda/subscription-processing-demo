using Camunda.Abstractions;
using Camunda.Command;
using SubscriptionService.Commands;
using Subscription = SubscriptionService.Model.Subscription;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionRepository repository;

    public SubscriptionController(SubscriptionRepository repository)
    {
        this.repository = repository;
    }
    
    [HttpGet("{subscriptionId}")]
    public async Task<ActionResult<SubscriptionModel>> Get(string subscriptionId)
    {
        Guard.ArgumentNotNullOrEmpty(subscriptionId);

        var subscription = await repository.GetAsync(subscriptionId);
        if (subscription == null)
            return NotFound();

        return Ok(subscription.ToModel());
    }
    
    // subscription processing
    [HttpPost]
    public async Task<ActionResult<int>> Register(
        [Required] RegisterSubscriptionRequestCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        // trigger processing in Camunda
        var response = await zeebeClient.CreateInstanceAsync(
            new CreateInstanceRequest("Subscription_Process_Workflow",
                null, null, command));

        return Ok(new{ response.ProcessInstanceKey });
    }
    
    [HttpPost("/register")]
    public async Task<ActionResult<SubscriptionModel>> Register(
        [Required] RegisterSubscriptionCommand command)
    {
        // register a new subscription
        var subscription = new Subscription(command.ProductId, command.LoanAmount, command.InsuredAmount);
        subscription.Register();
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        // set object back to Camunda
        return Ok(subscription.ToModel());
    }

    [HttpPost("/normalize")]
    public async Task<ActionResult<SubscriptionModel>> Normalize(
        [Required] NormalizeSubscriptionCommand command,
        [FromServices] ProductProxyService productService)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return NotFound();
        
        subscription.Normalize(productService);
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        return Ok(subscription.ToModel());
    }
    
    [HttpPost("/validate")]
    public async Task<ActionResult<SubscriptionModel>> Validate(
        [Required] ValidateSubscriptionCommand command,
        [FromServices] IZeebeClient zeebeClient,
        [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        var subscription = await repository.GetAsync(command.SubscriptionId);
        if (subscription == null)
            return BadRequest();

        subscription.CustomerId = command.CustomerId;
        var validationResult = subscription.Validate();
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        if (validationResult.IsValid) 
            return Ok(subscription.ToModel());

        // identify current job
        var jobKey = httpContextAccessor.HttpContext?.Request.Headers["X-Zeebe-Process-Instance-Key"];
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
        
        subscription.Accept("well, accepted!");
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

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
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

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
        await repository.AddAsync(subscription, command.ProcessInstanceKey);

        return Ok(subscription.ToModel());
    }
}