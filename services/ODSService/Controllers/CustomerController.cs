using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ODSService.Commands;

namespace ODSService.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> logger;
    private readonly ODSDataContext dataContext;
    private readonly CustomerQuery customerQuery;

    public SubscriptionController(
        ILogger<SubscriptionController> logger, 
        ODSDataContext dataContext,
        CustomerQuery customerQuery)
    {
        this.logger = logger;
        this.dataContext = dataContext;
        this.customerQuery = customerQuery;
    }

    [HttpGet]
    public async Task<ICollection<CustomerModel>> GetCustomers(int limit = 5)
    {
        return await customerQuery.GetCustomersAsync(limit);
    }

    [HttpGet("{customerId}/subscriptions")]
    public async Task<ICollection<SubscriptionModel>> GetSubscriptions(string customerId)
    {
        return await customerQuery.GetSubscriptionsForCustomer(customerId);
    }

    [HttpPost("/register-subscription-request")]
    public async Task<ActionResult> RegisterSubscriptionRequest(
        [Required] RegisterSubscriptionRequestCommand command)
    {
        var customer = await dataContext.FindAsync<Customer>(command.CustomerId);
        if (customer == null)
        {
            customer = new Customer
            {
                Id = command.CustomerId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                State = command.CustomerState,
                BirthDate = command.BirthDate
            };
        }

        customer.Subscriptions.Add(new Subscription
        {
            Id = command.SubscriptionId,
            InsuredAmount = command.InsuredAmount,
            LoanAmount = command.LoanAmount,
            LastUpdatedOn = command.RegisteredOn,
            ReceivedOn = command.RegisteredOn,
            State = command.CustomerState,
            ProductId = command.ProductId,
            ProcessInstanceKey = command.ProcessInstanceKey
        });

        customer.TotalLoanAmount += command.LoanAmount;
        customer.TotalInsuredAmount += command.InsuredAmount;

        await dataContext.AddAsync(customer);

        return Ok();
    }

    public async Task<ActionResult> SubscriptionAccepted(SubscriptionAcceptedCommand command)
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

    public async Task<ActionResult> SubscriptionRejected(SubscriptionRejectedCommand command)
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
    
    public async Task<ActionResult> SubscriptionSuspended(SubscriptionSuspendedCommand command)
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