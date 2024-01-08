using System.ComponentModel.DataAnnotations;

namespace Camunda.Command
{
    public record PublishMessageResponse([Required] long Key);
}