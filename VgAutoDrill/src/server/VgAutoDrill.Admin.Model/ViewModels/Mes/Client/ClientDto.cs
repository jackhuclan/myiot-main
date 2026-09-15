namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Client
{
    public class ClientDto : BaseDto
    {
        public virtual string? Code { get; set; }

        public virtual string? Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
    }
}
