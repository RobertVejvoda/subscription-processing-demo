using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ODSService.Commands;

namespace ODSService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> logger;
    private readonly OdsDataContext dataContext;
    private readonly CustomerQuery customerQuery;

    public CustomerController(
        ILogger<CustomerController> logger, 
        OdsDataContext dataContext,
        CustomerQuery customerQuery)
    {
        this.logger = logger;
        this.dataContext = dataContext;
        this.customerQuery = customerQuery;
    }

    [HttpGet]
    public async Task<ICollection<CustomerModel>> GetCustomers(int limit = 5)
    {
        return await customerQuery.FindCustomersAsync(limit);
    }

    [HttpGet("{customerId}/subscriptions")]
    public async Task<ICollection<SubscriptionModel>> GetSubscriptions(string customerId)
    {
        return await customerQuery.FindSubscriptionsForCustomer(customerId);
    }

    [HttpPost("/register-subscription")]
    public async Task<ActionResult> RegisterSubscription(
        [Required] SubscriptionRegisteredCommand command)
    {
        var customer = await dataContext.FindAsync<Customer>(command.CustomerId) ?? new Customer
        {
            Id = command.CustomerId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            State = command.CustomerState,
            BirthDate = command.BirthDate,
            Subscriptions = new List<Subscription>()
        };

        customer.Subscriptions.Add(new Subscription
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
        var subscription = await dataContext.FindAsync<Subscription>(command.SubscriptionId);
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
        var subscription = await dataContext.FindAsync<Subscription>(command.SubscriptionId);
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
        var subscription = await dataContext.FindAsync<Subscription>(command.SubscriptionId);
        if (subscription == null)
            return NotFound();

        subscription.UnderwritingResult = command.UnderwritingResult;
        subscription.Message = command.Message;
        subscription.LastUpdatedOn = command.SuspendedOn;

        await dataContext.SaveChangesAsync();
        
        return Ok();
    }
 }