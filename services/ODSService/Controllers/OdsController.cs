using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ODSService.Commands;
using Customer = ODSService.Model.Customer;
using Subscription = ODSService.Model.Subscription;

namespace ODSService.Controllers;

[ApiController]
[Route("/api")]
public class OdsController : ControllerBase
{
    private readonly OdsDataContext dataContext;
    private readonly Queries.Queries queries;

    public OdsController(
        OdsDataContext dataContext,
        Queries.Queries queries)
    {
        this.dataContext = dataContext;
        this.queries = queries;
    }

    [HttpGet("customers")]
    public async Task<ActionResult<ICollection<Customer>>> GetCustomers([FromQuery] int take = 15)
    {
        return Ok(await queries.GetCustomersAsync(take));
    }

    [HttpGet("customers/{customerId}/subscriptions")]
    public async Task<ActionResult<ICollection<Subscription>>> GetCustomerSubscriptions([Required] string customerId)
    {
        return Ok(await queries.GetCustomerSubscriptions(customerId));
    }

    [HttpGet("subscriptions")]
    public async Task<ActionResult<ICollection<Subscription>>> GetSubscriptions([FromQuery] int take = 15)
    {
        return Ok(await queries.GetSubscriptions(take));
    }

    [HttpGet("subscriptions/{processInstanceKey}")]
    public async Task<ActionResult<Subscription?>> GetSubscriptionByProcessInstanceKey([Required] string processInstanceKey)
    {
        var result = await queries.FindSubscriptionByProcessInstanceKey(processInstanceKey);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/register-subscription")]
    public async Task<ActionResult> RegisterSubscription(
        [Required] SubscriptionRegisteredCommand command)
    {
        var customer = await dataContext.FindAsync<Entity.Customer>(command.CustomerId) ?? new Entity.Customer
        {
            Id = command.CustomerId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            State = command.CustomerState,
            BirthDate = command.BirthDate,
            Subscriptions = new List<Entity.Subscription>()
        };

        customer.Subscriptions.Add(new Entity.Subscription
        {
            Id = command.SubscriptionId,
            InsuredAmount = command.InsuredAmount,
            LoanAmount = command.LoanAmount,
            LastUpdatedOn = command.RegisteredOn,
            ReceivedOn = command.RegisteredOn,
            State = command.CustomerState,
            ProductId = command.ProductId,
            ProcessInstanceKey = command.ProcessInstanceKey,
        });

        customer.TotalLoanAmount += command.LoanAmount;
        customer.TotalInsuredAmount += command.InsuredAmount;

        await dataContext.AddAsync(customer);

        return Ok();
    }

    [HttpPost("/accept-subscription")]
    public async Task<ActionResult> AcceptSubscription(SubscriptionAcceptedCommand command)
    {
        var subscription = await dataContext.FindAsync<Entity.Subscription>(command.SubscriptionId);
        if (subscription == null)
            return NotFound();

        subscription.UnderwritingResult = command.UnderwritingResult;
        subscription.Message = command.Message;
        subscription.LastUpdatedOn = command.AcceptedOn;

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("/reject-subscription")]
    public async Task<ActionResult> RejectSubscription(SubscriptionRejectedCommand command)
    {
        var subscription = await dataContext.FindAsync<Entity.Subscription>(command.SubscriptionId);
        if (subscription == null)
            return NotFound();

        subscription.UnderwritingResult = command.UnderwritingResult;
        subscription.Message = command.Message;
        subscription.LastUpdatedOn = command.RejectedOn;

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPost("/suspend-subscription")]
    public async Task<ActionResult> SuspendSubscription(SubscriptionSuspendedCommand command)
    {
        var subscription = await dataContext.FindAsync<Entity.Subscription>(command.SubscriptionId);
        if (subscription == null)
            return NotFound();

        subscription.UnderwritingResult = command.UnderwritingResult;
        subscription.Message = command.Message;
        subscription.LastUpdatedOn = command.SuspendedOn;

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
 }