using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{
    public static class EnvironmentVariables
    {
        public const string APP_ID = nameof(APP_ID);
        public const string APP_PORT = nameof(APP_PORT);
        public const string DAPR_HTTP_PORT = nameof(DAPR_HTTP_PORT);
        public const string DAPR_GRPC_PORT = nameof(DAPR_GRPC_PORT);
        public const string DAPR_METRICS_PORT = nameof(DAPR_METRICS_PORT);
        public const string NAMESPACE = nameof(NAMESPACE);
        public const string ASPNETCORE_URLS = nameof(ASPNETCORE_URLS);
        public const string ASPNETCORE_ENVIRONMENT = nameof(ASPNETCORE_ENVIRONMENT);
    }
}