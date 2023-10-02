using Healthchecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<SubscriptionIntegrationEventHandler>();
builder.Services.AddScoped<IEventBus, DaprEventBus>();

// Add services to the container.
builder.Services.AddControllers().AddDapr(builder =>
{
    builder.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web));
});
builder.Services.AddDaprClient();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprSidekick(builder.Configuration);
}

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDapr();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" }); });


var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API - v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.MapSubscribeHandler();
app.MapControllers();

app.Run();