using System.Configuration;
using BlazorDownloadFile;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using VgCNCServer.DAO;
using VgCNCServer.Razor;
using VgEAPClient.Common;

#region 控制台输出Logo

Console.WriteLine("starting.............");
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Suzhou Vega Technology Co., Ltd ");
Console.WriteLine("Vega Gateway Pro");
Console.WriteLine("设备采集，多向扩展");
Console.WriteLine("##作者## zhushipeng");
Console.WriteLine($"##当前版本## {Assembly.GetExecutingAssembly().GetName().Version}");
Console.WriteLine("##文档地址## http://localhost:5001/swagger/index.html");
Console.ResetColor();

#endregion 控制台输出Logo

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()   //添加 Razor 组件的服务。
    .AddInteractiveServerComponents();  //添加服务以支持呈现交互式服务器组件。

builder.Services.AddMasaBlazor(options =>
{
    options.ConfigureTheme(theme =>
    {
        theme.Themes.Light.Primary = "#4318FF";
        theme.Themes.Light.Accent = "#4318FF";
    });
}).AddI18nForServer("wwwroot/i18n");

builder.Services.Configure<EAPClientOptions>(builder.Configuration.GetSection(nameof(EAPClientOptions)));

builder.Services.AddGlobalForServer();
builder.Services.AddBlazorDownloadFile();

builder.Services.AddDbContextFactory<VegaContext>(optionsBuilder =>
{
    optionsBuilder.EnableSensitiveDataLogging(true);
    string? connection = builder.Configuration.GetSection("ConnectionSetting:ConnectionString").Value;
    if (!string.IsNullOrWhiteSpace(connection))
    {
        optionsBuilder.UseMySQL(connection, providerOptions => providerOptions.CommandTimeout(60));
    }
});

builder.Services.AddWindowsService();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseNLog();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//终结点约定生成器扩展：
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();  //为应用配置交互式服务器端呈现（交互式 SSR）。

// 设置开机自启动
//StartupTaskCreator.SetupAutoStart();

app.Run();
