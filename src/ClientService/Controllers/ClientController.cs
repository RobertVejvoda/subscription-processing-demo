namespace ClientService.Controllers;

[ApiController]
[Route("/api/clients")]
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> logger;

    public ClientController(ILogger<ClientController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<string>> GetClient(
        [Required, FromQuery] string clientId, 
        [FromServices] ClientRepository repository)
    {
        var client = await repository.GetAsync(clientId);
        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpPost("OnSubscriptionRequestReceived")]
    [Topic(Resources.Bindings.PubSub, nameof(SubscriptionRequestReceivedIntegrationEvent))]
    public Task HandleAsync([Required] SubscriptionRequestReceivedIntegrationEvent @event,
        [FromServices] SubscriptionIntegrationEventHandler handler)
        => handler.Handle(@event);
}