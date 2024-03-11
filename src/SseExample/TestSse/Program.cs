using Microsoft.AspNetCore.Builder;
using TestSse;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddSingleton<ISseHolder, SseHolder>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.Map("/sse/connect", (app) => app.UseMiddleware<SseMiddleware>());

app.MapControllers();

app.UseCors((corsPolicyBuilder) =>
             corsPolicyBuilder
             .AllowAnyHeader()
             .AllowAnyMethod()
             .SetIsOriginAllowed((string origin) => true)
             .AllowCredentials()
             .SetPreflightMaxAge(TimeSpan.FromDays(365)));


app.Run();
