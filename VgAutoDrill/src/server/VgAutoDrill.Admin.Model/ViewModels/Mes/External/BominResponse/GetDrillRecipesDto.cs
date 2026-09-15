namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse
{
    public class GetDrillRecipesDto
    {
        public DrillRecipesData? Data { get; set; } = new DrillRecipesData();
    }

    public class DrillRecipesData
    {
        public string? Lot { get; set; } = string.Empty;

        public string? DeviceId { get; set; } = string.Empty;

        public string? ProcessCode { get; set; } = string.Empty;

        /// <summary>
        /// 钻孔参数文件地址
        /// </summary>
        public virtual string? DiaFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 钻孔程序路径
        /// </summary>
        public virtual string? ProgramFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp的host地址
        /// </summary>
        public virtual string? FtpHost { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp的host地址端口
        /// </summary>
        public virtual string? FtpPort { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp地址的用户名
        /// </summary>

        public virtual string? FtpUsername { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp地址的密码
        /// </summary>
        public virtual string? FtpPassword { get; set; } = string.Empty;


        public bool Success { get; set; }

        public string? Content { get; set; } = string.Empty;
    }
}
