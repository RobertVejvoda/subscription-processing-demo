namespace CustomerExperienceAPI.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerRequestController
    : ControllerBase
{
    private const string BpmnProcessId = "register-customer-process_1434vxu";
    
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(
        [Required] RegisterCustomerRequestCommand command,
        [FromServices] IZeebeClient zeebeClient)
    {
        // trigger processing in Camunda
        var request = new CreateInstanceWithResultRequest(BpmnProcessId, null, true,"10s", null, command);
        var response = await zeebeClient.CreateInstanceWithResultAsync(request);

        var customerIdModel = JsonSerializer.Deserialize<CustomerIdModel>(response.Variables!, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        
        return Ok(customerIdModel);
    }
 }