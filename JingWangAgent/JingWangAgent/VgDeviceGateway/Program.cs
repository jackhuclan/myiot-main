using Microsoft.Extensions.Caching.Memory;
using NLog.Web;
using Vegalot.External.XianjinIot.Common;
using VgAutoDrill.Admin.WebApi.Configurations;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Infrastructure.Plugin;

namespace VgDeviceGateway
{
#pragma warning disable S1118 // Utility classes should not have public constructors

    public class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", false);
            int? port = builder.Configuration.GetValue<int?>("port");
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(port.HasValue ? port.Value : 8004);
            });
            builder.Services.Configure<MemoryCacheOptions>(builder.Configuration.GetSection(nameof(MemoryCacheOptions)));
            var memoryCacheOptions = builder.Configuration.GetSection(nameof(MemoryCacheOptions)).Get<MemoryCacheOptions>();
            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddDevices(builder.Configuration);
            builder.Services.AddPlugin(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Swagger Config
            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddWindowsService();
            //// NLog: Setup NLog for Dependency injection
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.MapControllers();

            app.Run();

            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += (ctx) =>
          {
              //Log.Information("Closing");
              System.Console.WriteLine("Closing");
              //WaitThreadExit(app);
              app.StopAsync().Wait();
          };
        }

        private static void WaitThreadExit(WebApplication app)
        {
            app.StopAsync().Wait();
        }
    }
}