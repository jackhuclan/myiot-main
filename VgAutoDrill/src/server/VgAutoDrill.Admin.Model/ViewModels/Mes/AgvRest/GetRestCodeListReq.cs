namespace VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest
{
    public class GetRestCodeListReq : Page
    {

        /// <summary>
        /// 休息点编码
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 休息点名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 物理点位
        /// </summary>
        public virtual string? Point { get; set; }

        /// <summary>
        /// 该分区该点预定分配的AGV
        /// </summary>
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 当前点正占用的AGV
        /// </summary>
        public virtual string? CurrentAgv { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;

    }
}
