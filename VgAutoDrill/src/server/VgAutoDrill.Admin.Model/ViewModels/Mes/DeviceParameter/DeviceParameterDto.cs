namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceParameter
{
    public class DeviceParameterDto : BaseDto
    {
        public int DeviceTypeId { get; set; }

        public int DeviceId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }

        /// <summary>
        /// 参数
        /// json 字符串
        /// </summary>
        public virtual string? Parameters { get; set; }
    }
}
