using Microsoft.AspNetCore.StaticFiles;
using NLog.Web;

namespace VgAutoUpdaterHub;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        int? port = builder.Configuration.GetValue<int?>("port");
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(port.HasValue ? port.Value : 5258);
        });
        builder.Services.AddCors();

        // NLog: Setup NLog for Dependency injection
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Host.UseNLog();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            builder.Services.AddWindowsService();
        }

        var app = builder.Build();

        var fileServerOptions = new FileServerOptions
        {
            FileProvider = builder.Environment.WebRootFileProvider,
            EnableDefaultFiles = true,
            EnableDirectoryBrowsing = true,
        };

        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings.Add(".bat", "application/octet-stream");
        contentTypeProvider.Mappings.Add(".ps1", "application/octet-stream");
        fileServerOptions.StaticFileOptions.ContentTypeProvider = contentTypeProvider;
        app.UseFileServer(fileServerOptions);

        app.Map("/health", () => "ok");

        app.Run();
    }
}
