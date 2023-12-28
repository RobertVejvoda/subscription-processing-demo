var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddDapr(client => client.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web)));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ODSDataContext>()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDapr();

builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<CustomerQuery>();
builder.Services.AddDbContext<ODSDataContext>(
    options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("ODSDataMartConnectionString"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODS API - v1");
    c.RoutePrefix = string.Empty;
});


app.UseHealthChecks("/healthz");
app.UseCloudEvents();

app.MapSubscribeHandler();
app.MapControllers();

app.Run();