using Platform.Infrastructure.Authentication;
using ProductExample.CommandWebHost.Extensions;
using Platform.Infrastructure.Bus.RabbitMQ.Extensions;
using Platform.Infrastructure.Host.WebApi.Extensions;
using Platform.Infrastructure.Host.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.AddJwtBearer(builder.Configuration);

builder.Services.AddCoreServices(new HostServiceConfig { UseEndpointProtection = false });

builder.Services.AddServiceBusProvider(builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpPipeline(enableAuthorization: false, enableServiceIdCheckerMiddleware: false, enableTenantIdCheckerMiddleware: false);


app.Run();
logger.LogInformation("Application started");
