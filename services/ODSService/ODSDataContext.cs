namespace ODSService;

public class ODSDataContext : DbContext
{
    private const string DEFAULT_SCHEMA = "dbo";
    private readonly ILoggerFactory loggerFactory;
    
    public ODSDataContext(DbContextOptions<ODSDataContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        this.loggerFactory = loggerFactory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment))
            environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        optionsBuilder
            .UseSqlServer("name=ConnectionStrings:ODSDataMartConnectionString", providerOptions => { providerOptions.EnableRetryOnFailure(); })
            .UseLoggerFactory(loggerFactory);

        if (environment == "Development")
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        modelBuilder.HasSequence<int>("CustomerNumbers").StartsAt(100).IncrementsBy(1);
        modelBuilder.HasSequence<int>("SubscriptionNumbers").StartsAt(100).IncrementsBy(1);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ODSDataContext).Assembly);
    }
}