using NLog.Web;
using VgAutoDrill.DataCollect.Application;
using VgAutoDrill.DataCollect.Application.Interfaces;
using VgAutoDrill.DataCollect.Application.Services;
using VgAutoDrill.DataCollect.WebApi.Configurations;

namespace VgAutoDrill.DataCollect.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", false);
            string portInput = builder.Configuration["AppConfig:Port"];
            if (!int.TryParse(portInput, out var port))
            {
                port = 8002;
            }
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(port);
            });

            builder.Services.AddControllers();
            //click house db config
            builder.Services.Configure<ClickhouseOptions>(builder.Configuration.GetSection(ClickhouseOptions.Options));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Swagger Config
            builder.Services.AddSwaggerConfiguration();
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Host.UseNLog();

            builder.Services.AddSingleton<IDataCollectService, DataCollectService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}