using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using NLog.Web;

namespace VgAutoUpdaterHub;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        int? port = builder.Configuration.GetValue<int?>("port");
        string? DrillFilesRoot = builder.Configuration.GetValue<string?>("DrillFilesRoot");
        string? AllowFileExt = builder.Configuration.GetValue<string?>("AllowFileExt");
        if (string.IsNullOrEmpty(DrillFilesRoot)) throw new Exception("please configure DrillFilesRoot");
        if (string.IsNullOrEmpty(AllowFileExt) || AllowFileExt.ToLower() == "*")
            AllowFileExt = "";

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(port.HasValue ? port.Value : 6258);
        });
        builder.Services.AddCors();
        builder.Services.AddControllers();

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
            FileProvider = new PhysicalFileProvider(DrillFilesRoot),
            EnableDefaultFiles = true,
            EnableDirectoryBrowsing = true,
        };

        var contentTypeProvider = new FileExtensionContentTypeProvider();
        foreach (var fileExt in AllowFileExt.Split(";"))
        {
            contentTypeProvider.Mappings.Add(fileExt, "application/octet-stream");
        }
        fileServerOptions.StaticFileOptions.ContentTypeProvider = contentTypeProvider;
        app.UseFileServer(fileServerOptions);

        app.MapControllers();
        app.Run();
    }
}
