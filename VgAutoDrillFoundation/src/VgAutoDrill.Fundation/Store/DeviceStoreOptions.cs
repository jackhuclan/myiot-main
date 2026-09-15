namespace VgAutoDrill.Fundation.Store;

public class DeviceStoreOptions
{
    public DeviceStoreKind StoreType { get; set; } = DeviceStoreKind.Local;
    /// <summary>
    /// 缓存存放路径, 默认存放应用程序根路径
    /// </summary>
    public string SavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
    public bool PanelSaveEnabled { get; set; } = true;
    public bool CutterSaveEnabled { get; set; } = true;
    /// <summary>
    /// 追踪ScheduleTasks调度任务
    /// </summary>
    public bool TrackScheduleTasks { get; set; } = true;
    public bool Enabled { get; set; } = false;
}
