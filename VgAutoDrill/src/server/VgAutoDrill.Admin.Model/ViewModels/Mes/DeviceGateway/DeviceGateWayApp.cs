namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway
{
    public class DeviceGateWayApp : IEquatable<DeviceGateWayApp>
    {
        public string AppName { get; set; } = string.Empty;
        public string ZipName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public Version AppVersion { get; set; } = new Version();
        public string Hash { get; set; } = string.Empty;
        /// <summary>
        /// 升级或者降级到指定版本，默认为空，不指定版本
        /// </summary>
        public Version? TargetVersion { get; set; }
        public string InstalledLocation { get; set; } = string.Empty;
        /// <summary>
        /// 安装之前执行脚本
        /// </summary>
        public List<string> PreScripts { get; set; } = new();
        /// <summary>
        /// 安装后执行脚本
        /// </summary>
        public List<string> PostScripts { get; set; } = new();
        public bool EnableUpdate { get; set; } = true;
        public bool Downloaded { get; set; } = false;
        public bool Installed { get; set; } = false;
        public bool Equals(DeviceGateWayApp? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return this.AppName == other.AppName && this.AppVersion == other.AppVersion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.AppName, this.AppVersion);
        }
    }
}
