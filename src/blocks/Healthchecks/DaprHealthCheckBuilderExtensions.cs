using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healthchecks
{
    public static class DaprHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddDapr(this IHealthChecksBuilder builder) =>
            builder.AddCheck<DaprHealthCheck>("dapr");
    }
}