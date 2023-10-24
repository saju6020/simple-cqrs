using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Infrastructure.Host.WebApi.Extensions;
using SimpleCQRS.UAM.Database;
using System.Text;
using SimpleCQRS.UAM.Mapper;
using UAM.WebService.Configuration;
using Platform.Infrastructure.Core.Extensions;
using Validation.FluentValidationProvider.Extensions;
using Platform.Infrastructure.Common.Security;
using SimpleCQRS.UAM.Domain.Extension;
using Platform.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpComponents();

builder.Services.AddDbContext<UAMDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UAMDatabase")));

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<UAMDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMapperProfiles();

builder.Services.AddCore()
    .AddDefaultBusProvider()
    .AddDefaultDomainStore()
    .AddFluentValidation(options =>
    {
        options.ValidateAllCommands = true;
        options.ValidateAllQuery = false;
    })
    .AddDomainComponents();


builder.Services.AddScoped<UserContext>(p => new UserContext
{
    UserId = Guid.Parse(SimpleCQRS.UAM.Common.UAMConstants.TEST_ADMIN_USER_ID),
    TenantId = Guid.Parse(SimpleCQRS.UAM.Common.UAMConstants.TEST_TENANT_ID),
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();


var jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");
builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);
var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtBearerTokenSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtBearerTokenSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

app.UseHttpPipeline(false, false, false);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();
