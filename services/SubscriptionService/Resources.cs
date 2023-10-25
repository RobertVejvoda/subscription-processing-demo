namespace SubscriptionService;

public static class Resources
{
    public static class Bindings
    {
        public const string PubSub = "pubsub";
        public const string StateStore = "statestore";
    }

    public static class Topics
    {
        public static class Customer
        {
            public const string Registered = "customer-registered";
        }

        public static class Subscription
        {
            public const string Received = "subscription-received";
            public const string AssessmentRequested = "subscription-assessment-requested";
            public const string AssessmentFinished = "subscription-assessment-finished";
        }
    }

    public static class Apps
    {
        public const string CustomerService = "customer-service";
    }
}