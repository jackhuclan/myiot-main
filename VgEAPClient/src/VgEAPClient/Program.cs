// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using VegaUI;
using VgCNCServer.DAO;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Inbound;
using VgEAPClient.Common.Communication.Outbound;
using VgEAPClient.Common.Services;
using VgEAPClient.Common.Util;
using WindowsFormsLifetime;

namespace VgEAPClient;

internal static class Program
{
    public static List<VegaLanguage> VegaLanguages = new List<VegaLanguage>();
    public static string CurLanguage = "";

    private static readonly Mutex mutex = new Mutex(true, "维嘉DNC");

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        if (mutex.WaitOne(TimeSpan.Zero, true))
        {
            ApplicationConfiguration.Initialize();

            var host = Host.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration((hostBuilderContext, builder) =>
                  {
                      builder.AddJsonFile($"./appsettings.json");
                  })
                  .ConfigureServices((hostBuilderContext, services) =>
                  {
                      services.AddEAPClientCore(hostBuilderContext.Configuration);
                      services.AddTransient<frmShowDia>();
                      services.AddTransient<FrmBrokenKnifeReport>();
                      services.AddTransient<frmLogin>();
                      services.AddSingleton<IMessageBoxService, MessageBoxService>();
                      services.AddSingleton<SetuoWindowOpenRun>();
                      services.AddSingleton<Test>();

                      services.AddDbContextFactory<VegaContext>(optionsBuilder =>
                      {
                          optionsBuilder.EnableSensitiveDataLogging(true);
                          string? connection = hostBuilderContext.Configuration.GetSection("ConnectionSetting:ConnectionString").Value;
                          if (!string.IsNullOrWhiteSpace(connection))
                          {
                              optionsBuilder.UseMySQL(connection, providerOptions => providerOptions.CommandTimeout(60));
                          }
                      });
                      services.AddDbContextFactory<VgAOIContext>();
                  })
                  .ConfigureLogging(loggingBuilder =>
                  {
                      loggingBuilder.AddNLog();
                  })
                  .ConfigureWebHost(webbuilder =>
                  {
                      webbuilder.UseKestrel();
                      IConfigurationBuilder builder = new ConfigurationBuilder();
                      builder.AddJsonFile($"./appsettings.json");
                      var config = builder.Build();
                      bool Enabled = config.GetValue<bool>("HttpDataReceiverOptions:Enabled");
                      string UrlPrefix = config.GetValue<string>("HttpDataReceiverOptions:UrlPrefix") ?? "";
                      if (Enabled && !UrlPrefix.IsNullOrEmpty())
                      {
                          webbuilder.UseUrls(UrlPrefix);
                          webbuilder.Configure(app =>
                          {
                              var eQPDataReceiver = app.ApplicationServices.GetRequiredService<IEQPDataReceiver>();
                              eQPDataReceiver.Configure(webbuilder, app);
                          });
                      }
                      else
                      {
                          webbuilder.Configure(app => { });
                      }

                  })
                  .UseWindowsFormsLifetime<FrmMain>()
                  .Build();

            var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            var heartbeater = host.Services.GetRequiredService<IEAPHeartbeater>();
            heartbeater.StartAsync(applicationLifetime.ApplicationStopping);

            RunDataReporter(host, applicationLifetime.ApplicationStopping);
            RunDataCollector(host, applicationLifetime.ApplicationStopping);

            LoadLanguage("", true);

            host.Run();
            mutex.ReleaseMutex();
        }
        else
        {
            MessageBox.Show("维嘉DNC软件已经在运行!");
        }
    }

    private static void RunDataCollector(IHost host, CancellationToken applicationStopping)
    {
        var dataCollectorStarter = host.Services.GetRequiredService<IDataCollectorStarter>();
        dataCollectorStarter.Start();
    }

    private static void RunDataReporter(IHost host, CancellationToken cancellationToken)
    {
        var dataReporters = host.Services.GetServices<IEQPDataReporter>();
        if (dataReporters.Any())
        {
            foreach (var item in dataReporters)
            {
                item.StartAsync(cancellationToken);
            }
        }
    }

    private static void InitLanguage()
    {
        VegaLanguages = [];
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 连接错误", en = "CNC Connection error" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 断开错误", en = "CNC Disconnecting error" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 连接关闭", en = "CNC connection closed" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 连接异常", en = "CNC Connection exception" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 连接失败", en = "CNC Connection failed" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 已断开", en = "CNC Disconnected" });
        VegaLanguages.Add(new VegaLanguage() { Default = "CNC 连接成功", en = "CNC Connection successful" });
        VegaLanguages.Add(new VegaLanguage() { Default = "搜索程序文件夹路径{0}不存在", en = "The search program folder path {0} does not exist" });
        VegaLanguages.Add(new VegaLanguage() { Default = "在路径{0}未找到符合的文件名[{1}]", en = "No matching file name [{1}] was found at path {0}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "未加载", en = "Not loaded" });
        VegaLanguages.Add(new VegaLanguage() { Default = "为空", en = "Is Empty" });
        VegaLanguages.Add(new VegaLanguage() { Default = "苏州维嘉科技股份有限公司", en = "SuZhouVega" });
        VegaLanguages.Add(new VegaLanguage() { Default = "启动", en = "Start" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已从数据库中读取并加载程式：{0},内码：{1}", en = "Read and load program from database: {0}, inner code: {1}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已从数据库中读取开始运行程式,内码：{0}", en = "Started program read from database, internal code: {0}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已从数据库中读取停止运行程式,内码：{0}", en = "Stopped program read from database, internal code: {0}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "无法解析的类型：{0},内码：{1}", en = "Unresolvable type: {0}, inner code: {1}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已从数据库中读取并废弃类型：{0},内码：{1}", en = "Read and discard type from database: {0}, inner code: {1}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "是否确认退出【{0}】程序？", en = "Are you sure you want to exit the [{0}] program?" });
        VegaLanguages.Add(new VegaLanguage() { Default = "重要提示", en = "Important Notice" });
        VegaLanguages.Add(new VegaLanguage() { Default = "请输入工单号", en = "Please enter the work order number" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已请求工单下发", en = "Request for work order issuance" });
        VegaLanguages.Add(new VegaLanguage() { Default = "详细路径", en = "Detailed path" });
        VegaLanguages.Add(new VegaLanguage() { Default = "参数已经保存,需要重启软件。", en = "The parameters have been saved and the software needs to be restarted." });
        VegaLanguages.Add(new VegaLanguage() { Default = "【是】将会重启本系统", en = "[Yes] will restart this system" });
        VegaLanguages.Add(new VegaLanguage() { Default = "路径下没有相关文件，请排查", en = "There are no relevant files in the path, please check" });
        VegaLanguages.Add(new VegaLanguage() { Default = "直接加载文件", en = "Directly load files" });
        VegaLanguages.Add(new VegaLanguage() { Default = "加载", en = "Load" });
        VegaLanguages.Add(new VegaLanguage() { Default = "工单", en = "Wo" });
        VegaLanguages.Add(new VegaLanguage() { Default = "类型", en = "Type" });
        VegaLanguages.Add(new VegaLanguage() { Default = "点击加载文件", en = "Click to load file" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已禁止在机器报警状态下加载文件", en = "Loading files in machine alarm state has been prohibited" });
        VegaLanguages.Add(new VegaLanguage() { Default = "加工程序:", en = "PgmFile:" });
        VegaLanguages.Add(new VegaLanguage() { Default = "直径文件:", en = "DiaFile:" });
        VegaLanguages.Add(new VegaLanguage() { Default = "刀盘文件:", en = "AtpFile:" });
        VegaLanguages.Add(new VegaLanguage() { Default = "是否将 {0} 同时加载进机台[{1}]", en = "Should {0} be loaded into the machine [{1}] simultaneously" });
        VegaLanguages.Add(new VegaLanguage() { Default = "是否同时开始运行程序机台[{0}]", en = "Do you want to start running the program machine [{0}] at the same time" });
        VegaLanguages.Add(new VegaLanguage() { Default = "是否同时停止运行程序机台[{0}]", en = "Do you want to stop running the program machine [{0}] at the same time" });
        VegaLanguages.Add(new VegaLanguage() { Default = "HTTP监听已开启,IP:{0},Port:{1}", en = "HTTP listening is enabled, IP:{0},Port:{1}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "HTTP监听未开启", en = "HTTP listening not enabled" });
        VegaLanguages.Add(new VegaLanguage() { Default = "开始保养", en = "PMStart" });
        VegaLanguages.Add(new VegaLanguage() { Default = "保养结束", en = "PMEnd" });
        VegaLanguages.Add(new VegaLanguage() { Default = "自动模式", en = "Auto" });
        VegaLanguages.Add(new VegaLanguage() { Default = "手动模式", en = "Manual" });
        VegaLanguages.Add(new VegaLanguage() { Default = "已连接", en = "Cnected" });
        VegaLanguages.Add(new VegaLanguage() { Default = "断开连接", en = "DisCnected" });
        VegaLanguages.Add(new VegaLanguage() { Default = "FTP连接失败,IP:{0},Port:{1},用户:{3},密码:{4},异常:{5}", en = "FTP connection failed, IP:{0},Port:{1}, User: {3}, Password: {4}, Exception: {5}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "FTP连接失败,IP:{0},Port:{1},用户:{3},密码:{4}", en = "FTP connection failed, IP:{0},Port:{1}, User: {3}, Password: {4}" });
        VegaLanguages.Add(new VegaLanguage() { Default = "请输入正确的用户名和密码", en = "Please enter the correct username and password" });
        VegaLanguages.Add(new VegaLanguage() { Default = "错误", en = "Error" });
        VegaLanguages.Add(new VegaLanguage() { Default = "班次切换完成,当前班次:{0}", en = "Shift switching completed, current shift: {0}" });
    }

    public static void LoadLanguage(string Language, bool IsInit)
    {
        if (IsInit)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile($"./appsettings.json");
            var config = builder.Build();
            Language = config.GetValue<string>("EAPClientOptions:Language") ?? "";
            InitLanguage();
        }
        switch (Language)
        {
            case "en":
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Language);
                break;
            default:
                break;
        }
        CurLanguage = Language;
    }

    /// <summary>
    /// 翻译
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string VgTs(this string value)
    {
        var lan = VegaLanguages.FirstOrDefault(o => o.Default == value);
        if (lan == null)
        {
            return value;
        }
        else
        {
            switch (CurLanguage)
            {
                case "en":
                    return lan.en;
                default:
                    return lan.Default;
            }
        }
    }
}
