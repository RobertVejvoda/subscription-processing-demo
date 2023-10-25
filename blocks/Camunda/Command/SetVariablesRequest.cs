using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record SetVariablesRequest(
        [Required] long? ElementInstanceKey,
        [Required] object Variables,
        bool? Local);
}