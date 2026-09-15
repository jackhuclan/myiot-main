namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceCutter
{
    public class GetCutterListReq : Page
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string? Name { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string? Code { set; get; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
