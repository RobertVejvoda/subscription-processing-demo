namespace SubscriptionService;

public sealed class DaprOptions
{
    public const string Dapr = "DaprBindings";
    public string StateStore { get; set; } = string.Empty;
    public string PubSub { get; set; } = string.Empty;
}