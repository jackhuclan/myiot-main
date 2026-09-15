using System.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VgCNCServer.DAO.Models;
using VgEAPClient.Common;

namespace VgCNCServer.DAO;

public class VegaContext : DbContext
{
    private readonly EAPClientOptions _eapClientOptions;

    public VegaContext(DbContextOptions<VegaContext> options, IOptions<EAPClientOptions> eapClientOptions) : base(options)
    {
        _eapClientOptions = eapClientOptions.Value;
    }

    public DbSet<sysDrillInformation> sysDrillInformations { get; set; }
    public DbSet<sysLoginLog> sysLoginLogs { get; set; }
    public DbSet<sysStaff> sysStaffs { get; set; }
    public DbSet<usrAlarm> usrAlarms { get; set; }
    public DbSet<usrCOMM> usrCOMMs { get; set; }
    public DbSet<usr485Comm> usr485Comms { get; set; }
    public DbSet<usrDuty> usrDutys { get; set; }
    public DbSet<usrEvent> usrEvents { get; set; }
    public DbSet<usrM54Event> usrM54Events { get; set; }
    public DbSet<usrRealTimeDuty> usrRealTimeDutys { get; set; }
    public DbSet<usrShiftDuty> usrShiftDutys { get; set; }
    public DbSet<usrShiftFinalDuty> usrShiftFinalDutys { get; set; }
    public DbSet<usrAnalysisDuty> usrAnalysisDutys { get; set; }
    public DbSet<usrToolsBroken> usrToolsBrokens { get; set; }
    public DbSet<usrToolsBrokenEnd> usrToolsBrokenEnds { get; set; }
    public DbSet<usrWorkingCondition> usrWorkingConditions { get; set; }

    public DbSet<device_maintenance> deviceMaintenanceSet { get; set; }
    public DbSet<device_odometer_history> deviceOdometerHistorySet { get; set; }
    public DbSet<device_period_statistic> devicePeriodStatisticSet { get; set; }
    public DbSet<device_state_snapshot> deviceStateSnapshotSet { get; set; }

    public DbSet<work_order> workOrderSet { get; set; }

    public DbSet<usrEquLoadFileLog> usrEquLoadFileLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        var dateConverter = new ValueConverter<DateOnly, DateTime>(
               v => v.ToDateTime(TimeOnly.MinValue),
               v => DateOnly.FromDateTime(v));

        modelBuilder.Entity<usrCOMM>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usr485Comm>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrAlarm>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrAnalysisDuty>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrDuty>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrEvent>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrM54Event>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrRealTimeDuty>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrShiftDuty>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrShiftFinalDuty>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrToolsBroken>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrToolsBrokenEnd>().Property(e => e.dtDate).HasConversion(dateConverter);
        modelBuilder.Entity<usrWorkingCondition>().Property(e => e.dtDate).HasConversion(dateConverter);

        modelBuilder.Entity<sysDrillInformation>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<sysLoginLog>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrAlarm>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrAnalysisDuty>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrCOMM>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrDuty>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrEvent>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrM54Event>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrRealTimeDuty>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrShiftDuty>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrShiftFinalDuty>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrToolsBroken>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrToolsBrokenEnd>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
        modelBuilder.Entity<usrWorkingCondition>(entity => { if (!IsIncludeEqpType()) { entity.Ignore(e => e.sEqpType); } else { entity.Property(e => e.sEqpType); } });
    }

    private bool IsIncludeEqpType()
    {
        return _eapClientOptions.IsIncludeEqpType;
    }

    /// <summary>
    /// 获取数据库时间
    /// </summary>
    public DateTime GetDbTime()
    {
        string sql = $@"SELECT sysdate() as CurDateTime";
        return Database.SqlQueryRaw<DbCurTime>(sql).First().CurDateTime;
    }

    /// <summary>
    /// 获取数据库指定表总条目数
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <returns></returns>
    public int GetDbTableTotalCnt(string TableName)
    {
        string sql = $@"SELECT COUNT(1) AS Cnt FROM {TableName}";
        return Database.SqlQueryRaw<DbTableCounts>(sql).First().Cnt;
    }
}
