using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NLog.Web;
using System.Text.Json.Serialization;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.WebApi.Configurations;
using VgAutoDrill.Admin.WebApi.Middlewares;
using VgAutoDrill.Admin.WebApi.ScheduleJobs;
var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "cors";

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

Console.WriteLine($"builder.Environment.EnvironmentName:{builder.Environment.EnvironmentName}");

string portInput = builder.Configuration["AppConfig:Port"];
if (!int.TryParse(portInput, out var port))
{
    port = 8003;
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(port);
});

builder.Services.AddSingleton(new AppSettingsHelper(builder.Configuration));
// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);
//click house db config
//builder.Services.Configure<ClickhouseOptions>(builder.Configuration.GetSection(ClickhouseOptions.Options));
// HttpClient Config
builder.Services.AddHttpClientConfiguration(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IAPIHelper, APIHelper>();
//Cors Conig
builder.Services.AddCors(c =>
{
    c.AddPolicy(corsPolicyName, policy =>
    {
        policy.SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
          .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.AllowTrailingCommas = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Swagger Config
builder.Services.AddSwaggerConfiguration();

// Jwt Config
builder.Services.AddJwtConfiguration();

// Job
builder.Services.AddScheduleConfiguration();

builder.Services.AddAdminPlugins(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapperConfiguration();

builder.Services.Configure<ApiBehaviorOptions>((o) =>
{
    o.SuppressModelStateInvalidFilter = true;
});

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging(logging =>
{
    logging.AddNLog("conf/NLog.config");
});
builder.Host.UseNLog();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();
});

builder.Services.AddResponseCaching();
builder.Services.AddAdminSetup(builder.Configuration);

var app = builder.Build();

app.UseResponseCaching();
app.UseMiddleware<ExceptionLogMiddleware>();
app.UseMiddleware<RequestResponseLogMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.ToLower() == "test")
{
    app.UseSwaggerSetup();
}

app.UseHttpsRedirection();

app.UseStaticHttpContext();

app.UseCors(corsPolicyName);

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.Run();