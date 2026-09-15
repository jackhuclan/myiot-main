using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace VgMySqlStore.Plugin.Models;

public class VegaContext : DbContext
{
    private readonly VgMySqlStoreOptions _mySqlStoreOptions;

    [ActivatorUtilitiesConstructor]
    public VegaContext(IOptions<VgMySqlStoreOptions> options)
    {
        _mySqlStoreOptions = options.Value;
    }
    //public VegaContext(DbContextOptions<VegaContext> options) : base(options)
    //{
    //}

    public DbSet<EAP_MATERIAL_DATA> EAP_MATERIAL_DATAs { get; set; }
    public DbSet<EAP_SIGNAL_STATUS> EAP_SIGNAL_STATUSs { get; set; }
    public DbSet<EAP_TASK_DATA> EAP_TASK_DATAs { get; set; }
    public DbSet<PC_DATA_ALARM> PC_DATA_ALARMs { get; set; }
    public DbSet<PC_DATA_COUNT> PC_DATA_COUNTs { get; set; }
    public DbSet<PC_DATA_CRAFT> PC_DATA_CRAFTs { get; set; }
    public DbSet<PC_SIGNAL_STATUS> PC_SIGNAL_STATUSs { get; set; }
    public DbSet<PC_DATA_STATUS> PC_DATA_STATUSs { get; set; }
    public DbSet<PC_DATA_EMP> PC_DATA_EMPs { get; set; }
    public DbSet<sysStaff> sysStaffs { get; set; }
    public DbSet<PC_DATA_SCAN> PC_DATA_SCAN { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //启用显示敏感数据
        optionsBuilder.EnableSensitiveDataLogging(true);

        //var SqliteConnectionString = "Filename=.\\vega.db"; //"Filename=E:\\vega.sqlite"; 
        //optionsBuilder.UseSqlite(SqliteConnectionString);

        var connectionString = new MySqlConnector.MySqlConnectionStringBuilder()
        {
            Server = _mySqlStoreOptions.Server,
            Port = _mySqlStoreOptions.Port,
            UserID = _mySqlStoreOptions.UserID,
            Password = _mySqlStoreOptions.Password,
            Database = _mySqlStoreOptions.Database
        }.ToString();
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 43)));

        //var SqlServerConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Vega;Persist Security Info=True;User ID=sa;Password=1;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";
        //optionsBuilder.UseSqlServer(SqlServerConnectionString, providerOptions => providerOptions.CommandTimeout(60));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
