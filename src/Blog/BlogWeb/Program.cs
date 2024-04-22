using Blog.ORM.Context;
using Microsoft.EntityFrameworkCore;
using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Host.WebApi.Extensions;
using Blog.Mapper;
using Platform.Infrastructure.Core.Extensions;
using Platform.Infrastructure.Validation.FluentValidationProvider.Extensions;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.Blog.Domain.Commands;
using SimpleCQRS.Blog.Domain.CommandHandlers;
using Platform.Infrastructure.Repository.EF;
using Platform.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpComponents();

builder.Services.AddDbContext<BlogDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UAMDatabase")));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMapperProfiles();

builder.Services.AddCore()
    .AddDefaultBusProvider()
    .AddDefaultDomainStore()
    .AddFluentValidation(options =>
    {
        options.ValidateAllCommands = false;
        options.ValidateAllQuery = false;
    });


builder.Services.AddScoped<UserContext>(p => new UserContext
{
    UserId = Guid.Parse("97f1c1d9-4219-4fc3-adf4-19fdb9dd8846"),
    TenantId = Guid.Parse("97f1c1d9-4219-4fc3-adf4-19fdb9dd8846"),
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();

builder.Services.AddScoped<ICommandHandlerAsync<CreatePostCommand>, CreatePostCommandHandler>();
builder.Services.AddScoped(typeof(IOrmRepository), typeof(Repository<BlogDbContext>));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpPipeline(false, false, false);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


    app.UseSwagger();
    app.UseSwaggerUI();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapHealthChecks("/health");

app.Run();
