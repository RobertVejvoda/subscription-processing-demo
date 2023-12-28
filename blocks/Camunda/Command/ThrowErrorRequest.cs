using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record ThrowErrorRequest(
        [Required] long? JobKey,
        [Required] string ErrorCode,
        string? ErrorMessage);

    public record ThrowErrorResponse;
}