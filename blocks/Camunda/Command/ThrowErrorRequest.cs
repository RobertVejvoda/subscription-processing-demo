using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camunda.Command
{
    public record ThrowErrorRequest(
        [Required] long? JobKey,
        [Required] string ErrorCode,
        string ErrorMessage);

    public record ThrowErrorResponse();
}