
using Platform.Infrastructure.Host.WebApi.Extensions;
using Platform.Infrastructure.EndpointRoleFeatureMap.Extensions;
using Platform.Infrastructure.Bus.RabbitMQ.Extensions;
using System.Runtime.CompilerServices;
using Platform.Infrastructure.Host.WebApi;
using Platform.Infrastructure.Host.Contracts;
using ProductExample.CommandWebHost.Extensions;
using Platform.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtBearer(builder.Configuration);

builder.Services.AddCoreServices(new HostServiceConfig { UseEndpointProtection=true});
builder.Services.AddServiceBusProvider(builder.Configuration);
builder.Services.RegisterServices();

var app = builder.Build();
app.UseHttpPipeline(enableAuthorization:true, enableServiceIdCheckerMiddleware:false,enableTenantIdCheckerMiddleware:false);


