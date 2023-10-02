using System.Text.Json;
using EventBus;
using Microsoft.OpenApi.Models;
using SubscriptionService.Events;
using SubscriptionService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<SubscriptionRequestRepository>();
builder.Services.AddScoped<NormalizationService>();
builder.Services.AddScoped<SubscriptionAssessmentService>();
builder.Services.AddScoped<SubscriptionIntegrationEventHandler>();
builder.Services.AddScoped<ClientIntegrationEventHandler>();
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Subscription API", Version = "v1" });
});


var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscription API - v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.MapSubscribeHandler(); 
app.MapControllers();

app.Run();