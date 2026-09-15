using Microsoft.EntityFrameworkCore;
using VgSQLServerStore.Plugin.Models;

namespace VgSQLServerStore.Plugin;

public class VegaContext : DbContext
{
    public VegaContext(DbContextOptions<VegaContext> options) : base(options)
    {
    }
    public DbSet<sysDrillInformation> sysDrillInformations { get; set; }
    public DbSet<sysLoginLog> sysLoginLogs { get; set; }
    public DbSet<sysStaff> sysStaffs { get; set; }
    public DbSet<usrAlarm> usrAlarms { get; set; }
    public DbSet<usrCOMM> usrCOMMs { get; set; }
    public DbSet<usrDuty> usrDutys { get; set; }
    public DbSet<usrEvent> usrEvents { get; set; }
    public DbSet<usrM54Event> usrM54Events { get; set; }
    public DbSet<usrRealTimeDuty> usrRealTimeDutys { get; set; }
    public DbSet<usrShiftDuty> usrShiftDutys { get; set; }
    public DbSet<usrAnalysisDuty> usrAnalysisDutys { get; set; }
    public DbSet<usrToolsBroken> usrToolsBrokens { get; set; }
    public DbSet<usrWorkingCondition> usrWorkingConditions { get; set; }
    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //启用显示敏感数据
        optionsBuilder.EnableSensitiveDataLogging(true);
        var SqliteConnectionString = "Filename=.\\VgCNCServer.db"; //"Filename=E:\\vega.sqlite"; 
        optionsBuilder.UseSqlite(SqliteConnectionString);
        //var SqlServerConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Vega;Persist Security Info=True;User ID=sa;Password=1;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";
        //optionsBuilder.UseSqlServer(SqlServerConnectionString, providerOptions => providerOptions.CommandTimeout(60));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }


    private void btnLogin_Click(object sender, EventArgs e)
    {
        if (false == string.IsNullOrWhiteSpace(txtUser.Text.Trim()))
        {
            using (var ctx = new VegaContext())
            {
                var user = ctx.sysStaffs.FirstOrDefault(s => s.sLoginName == txtUser.Text.Trim().ToLower());
                if (user != null)
                {
                    if (txtUser.Text.Trim().ToLower() == "admin")
                    {
                        this.Hide();
                        Config.Role = "admin";
                        frmMain frm = new frmMain();
                        //frm.WindowState = FormWindowState.Maximized;
                        frm.Show();
                        //frm.Hide();
                    }
                    else
                    {
                        this.Hide();
                        Config.Role = "user";
                        frmMain frm = new frmMain();
                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("用户名或密码不正确，您是否按下了 Caps Lock 键！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        else
        {
            MessageBox.Show("用户名不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    */
}
