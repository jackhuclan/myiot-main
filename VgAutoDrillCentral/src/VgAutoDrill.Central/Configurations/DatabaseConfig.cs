using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.IOC;
using VgAutoDrill.Admin.Common.Configuration;
using VgAutoDrill.Admin.Common.Log;

namespace VgAutoDrill.Central.Configurations;

public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var dbstring = AppSettingsHelper.GetContent<string>(["ConnectionStrings", "VgAutoDrillAuthDB"]);
        var slave1 = AppSettingsHelper.GetContent<string>(["ConnectionStrings", "VgAutoDrillSlave1"]);
        var slave2 = AppSettingsHelper.GetContent<string>(["ConnectionStrings", "VgAutoDrillSlave2"]);

        var iocConfig = new IocConfig()
        {
            //数据库连接
            ConnectionString = dbstring,
            //判断数据库类型
            DbType = AppSettingsHelper.GetContent<string>("ConnectionStrings", "DbType") == IocDbType.MySql.ToString() ? IocDbType.MySql : IocDbType.SqlServer,
            //是否开启自动关闭数据库连接-//不设成true要手动close
            IsAutoCloseConnection = true,
            SlaveConnectionConfigs = new List<IocConfig> { }
        };

        if (!string.IsNullOrWhiteSpace(slave1))
        {
            iocConfig.SlaveConnectionConfigs.Add(new IocConfig
            {
                ConnectionString = slave1,
                ConfigId = "slave1",
                DbType = iocConfig.DbType,
            });
        }

        if (!string.IsNullOrWhiteSpace(slave2))
        {
            iocConfig.SlaveConnectionConfigs.Add(new IocConfig
            {
                ConnectionString = slave2,
                ConfigId = "slave2",
                DbType = iocConfig.DbType,
            });
        }

        //注入 ORM
        SugarIocServices.AddSqlSugar(iocConfig);

        //设置参数
        services.ConfigurationSugar(db =>
            {
                db.CurrentConnectionConfig.InitKeyType = InitKeyType.Attribute;
                //db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                //{
                //    //判断是否开启redis设置二级缓存方式
                //    DataInfoCacheService = AppSettingsHelper.RedisUseCache ? (ICacheService)new SqlSugarRedisCache() : new SqlSugarMemoryCache()
                //};

                //执行SQL，可监控sql
                db.Aop.OnLogExecuting = (sql, p) =>
                {
                    LoggerHelper.Debug($"SqlSugar执行SQL:{sql}");
                };

                //执行SQL 错误事件
                db.Aop.OnError = (exp) =>
                        {
                            LoggerHelper.Error(exp, "SqlSugar执行SQL错误事件");
                        };
            });
    }
}
