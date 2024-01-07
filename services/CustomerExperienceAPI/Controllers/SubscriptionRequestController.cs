using System.ComponentModel.DataAnnotations;
using Camunda.Abstractions;
using Camunda.Command;
using CustomerExperienceAPI.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CustomerExperienceAPI.Controllers;

[ApiController]
[Route("/api/subscriptions")]
public class SubscriptionRequestController(
    AggregationDataContext dataContext,
    Queries.Queries queries,
    IDateTimeProvider dateTimeProvider)
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

    [HttpPost("/register")]
    public async Task<ActionResult> RegisterSubscription(
        [Required] RegisterSubscriptionRequestCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        var receivedOn = dateTimeProvider.Now();
        
        // trigger processing in Camunda
        var response = await zeebeClient.CreateInstanceAsync(
            new CreateInstanceRequest(BpmnProcessId, null, null, command));
        
        // save request
        var customer = new SubscriptionRequestEntity
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
            ProcessInstanceKey = response.ProcessInstanceKey.ToString()!
        };
        await dataContext.AddAsync(customer);

        return Ok(new { ProcessInstanceKey = response.ProcessInstanceKey.ToString() });
    }
    
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/subscription-registered")]
    public async Task<ActionResult> OnSubscriptionRegistered(SubscriptionRegisteredCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost("/subscription-accepted")]
    public async Task<ActionResult> AcceptSubscription(SubscriptionAcceptedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("/subscription-rejected")]
    public async Task<ActionResult> OnSubscriptionRejected(SubscriptionRejectedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost("/subscription-suspended")]
    public async Task<ActionResult> OnSubscriptionSuspended(SubscriptionSuspendedCommand command)
    {
        var subscription = await dataContext.FindAsync<SubscriptionRequestEntity>(command.ProcessInstanceKey);
        if (subscription == null)
            return NotFound();

        subscription.SubscriptionState = command.SubscriptionState;
        subscription.UnderwritingResultMessage = command.Reason;
        subscription.LastUpdatedOn = dateTimeProvider.Now();

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
 }