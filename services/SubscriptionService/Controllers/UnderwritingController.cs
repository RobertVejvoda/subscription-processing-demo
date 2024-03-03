using Microsoft.OpenApi.Extensions;
using SubscriptionService.Domain;
using SubscriptionService.Dto;

namespace SubscriptionService.Controllers;

[ApiController]
[Route("/api/underwriting")]
public class UnderwritingController(UnderwritingRepository repository) : ControllerBase
{
    // ZEEBE endpoints should start with root path /
    
    [HttpPost("/register-underwriting-request")]
    public async Task<ActionResult> RegisterUnderwritingRequest(
        [Required] RegisterUnderwritingRequestCommand command)
    {
        var underwritingRequest = await repository.GetAsync(command.RequestId);
        if (underwritingRequest == null)
            underwritingRequest = new UnderwritingRequest(command.RequestId, command.CustomerId, command.Age,
                command.InsuredAmount, UnderwritingResultState.Registered.GetDisplayName(), string.Empty);
        
        await repository.AddAsync(underwritingRequest);

        return Ok(new { underwritingRequest.RequestId });
    }
    
    [HttpPost("/request-information")]
    public async Task<ActionResult> RequestInformation(
        [Required] RequestInformationCommand command)
    {
        var underwriting = await repository.GetAsync(command.RequestId);
        if (underwriting == null)
            return NotFound();

        underwriting.UnderwritingResultState = command.UnderwritingResultState;
        underwriting.UnderwritingResultMessage = command.UnderwritingResultMessage;
        
        await repository.AddAsync(underwriting);

        return Ok(new { underwriting.RequestId });
    }

    [HttpPost("/on-information-received")]
    public async Task<ActionResult> InformationReceived(InformationReceivedCommand command)
    {
        var underwriting = await repository.GetAsync(command.RequestId);
        if (underwriting == null)
            return NotFound();

        underwriting.UnderwritingResultState = UnderwritingResultState.InEvaluation.GetDisplayName();
        
        await repository.AddAsync(underwriting);

        return Ok(new { underwriting.RequestId });
    }

    [HttpPost("/calculate-age")]
    public ActionResult<int> CalculateAge([Required] CalculateAgeCommand command)
        => Ok(new { Age = Calculator.CalculateAge(command.BirthDate) });
}