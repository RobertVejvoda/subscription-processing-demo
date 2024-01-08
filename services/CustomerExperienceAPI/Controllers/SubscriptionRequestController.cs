namespace CustomerExperienceAPI.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionRequestController(CustomerDataContext dataContext, Queries.Queries queries, IDateTimeProvider dateTimeProvider)
    : ControllerBase
{
    private const string BpmnProcessId = "Subscription_Process_Workflow";
    
    [HttpGet]
    public async Task<ActionResult<ICollection<SubscriptionRequestModel>>> GetSubscriptions([FromQuery] int take = 15)
    {
        return Ok(await queries.GetSubscriptionRequests(take));
    }

    [HttpGet("{processInstanceKey}")]
    public async Task<ActionResult<SubscriptionRequestModel>> GetSubscriptionByProcessInstanceKey([Required] string processInstanceKey)
    {
        var result = await queries.FindSubscriptionRequestByProcessInstanceKey(processInstanceKey);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ProcessInstanceKeyModel>> RegisterSubscription(
        [Required] RegisterSubscriptionRequestCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        var receivedOn = dateTimeProvider.Now();
        
        // trigger processing in Camunda
        var response = await zeebeClient.CreateInstanceAsync(
            new CreateInstanceRequest(BpmnProcessId, null, null, command));
        
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
            ProcessInstanceKey = response.ProcessInstanceKey.ToString()
        };
        await dataContext.AddAsync(subscriptionRequest);
        await dataContext.SaveChangesAsync();

        return Ok(new ProcessInstanceKeyModel(response.ProcessInstanceKey.ToString()));
    }
}