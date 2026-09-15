using NLog.Web;
using VgAutoDrill.Admin.WebApi.Configurations;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Agent
{
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        public static void Main(string[] args)
        {
            var portInput = args.FirstOrDefault(x => x.ToLower().StartsWith("port="));
            int port = 8004;
            if (portInput != null)
            {
                Console.WriteLine($"portInput:{portInput}");
                var portValue = portInput.Split("=", StringSplitOptions.RemoveEmptyEntries)[1];
                if (!int.TryParse(portValue, out port))
                {
                    Console.WriteLine($"port input parameter error");
                    return;
                }
            }
            else
            {
                Console.WriteLine("no port parameter found.Set to 8004 by default.");
            }

            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile($"./conf/appsettings.{builder.Environment.EnvironmentName}.json", false);
            //start处理localhost能调通，192.168.0.107 不能调通的问题 
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(port);
            });
            //end处理localhost能调通，192.168.0.107 不能调通的问题 
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddDevices(builder.Configuration);
            builder.Services.AddControllers();
            //111111111111111111111111
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Swagger Config
            builder.Services.AddSwaggerConfiguration();

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Host.UseNLog();

            //11111111111111111111111
            var app = builder.Build();
            //11111111111111111111
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //11111111111111111111111111
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
        }
    }
}