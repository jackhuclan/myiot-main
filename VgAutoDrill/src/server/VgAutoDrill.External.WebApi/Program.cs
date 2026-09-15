using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NLog.Web;
using System.Text.Json.Serialization;
using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.WebApi.Configurations;
using VgAutoDrill.External.WebApi.Configurations;
using VgAutoDrill.External.WebApi.ScheduleJobs;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "cors";

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

string portInput = builder.Configuration["AppConfig:Port"];
if (!int.TryParse(portInput, out var port))
{
    port = 8005;
}
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(port);
});

builder.Services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
          .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.AllowTrailingCommas = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
}).ConfigureApiBehaviorOptions((options) =>
{
    options.InvalidModelStateResponseFactory = (context) =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState);
        var result = new ResponseDto<string>()
        {
            Code = ResponseCode.Fail,
            Message = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
        };
        return new JsonResult(result);
    };
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();
});

builder.Services.AddSingleton(new AppSettingsHelper(builder.Configuration));
// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);
// AutoMapper
builder.Services.AddAutoMapperConfiguration();
// Swagger Config
builder.Services.AddSwaggerConfiguration();
// Jwt Config
builder.Services.AddJwtConfiguration();
// Job
builder.Services.AddScheduleConfiguration();

builder.Services.AddExternalPlugins(builder.Configuration);

builder.Services.AddCors();
// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging(logging =>
{
    logging.AddNLog("conf/NLog.config");
});
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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