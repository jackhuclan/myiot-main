using Newtonsoft.Json.Serialization;
using NLog.Web;
using VgAutoUpdater.Core;

namespace VgAutoUpdater;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        int? port = builder.Configuration.GetValue<int?>("port");
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(port.HasValue ? port.Value : 5257);
        });
        builder.Services.AddCors();
        // Add services to the container.
        builder.Services.AddUpdater(builder.Configuration);
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // NLog: Setup NLog for Dependency injection
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Host.UseNLog();
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            builder.Services.AddWindowsService();
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
