using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camunda.Command
{
    public record CancelInstanceRequest(long? ProcessInstanceKey);
}