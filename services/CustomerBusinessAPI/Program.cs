using Camunda;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
builder.Services.AddScoped<IZeebeClient, ZeebeClient>();
builder.Services.AddScoped<Queries>();

builder.Services.AddControllers()
    .AddDapr(client => client.UseJsonSerializationOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web)));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CustomerDataContext>()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDapr();

builder.Services.AddDbContext<CustomerDataContext>(
    options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("CustomerDataStore")))
        .AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Experience API - v1");
    c.RoutePrefix = string.Empty;
});


app.UseHealthChecks("/healthz");

app.MapControllers();

app.Run();