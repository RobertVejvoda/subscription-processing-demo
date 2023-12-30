var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
builder.Services.AddScoped<CustomerQuery>();

builder.Services.AddControllers()
    .AddDapr(client => client.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web)));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<OdsDataContext>()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDapr();

builder.Services.AddDbContext<OdsDataContext>(
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

app.MapControllers();

app.Run();