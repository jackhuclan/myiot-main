namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceCutter
{
    public class CutterDto : BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 最高库存
        /// </summary>
        public int? MaxStock { get; set; }

        /// <summary>
        /// 现有库存
        /// </summary>
        public int? CurrentStock { get; set; }

        /// <summary>
        /// 最低库存
        /// </summary>
        public int? MinStock { get; set; }

        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime? InboundDate { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 刀片寿命
        /// </summary>
        public virtual int? Life { get; set; }

        /// <summary>
        /// 预计加工件数
        /// </summary>
        public virtual int? EstimatedPieces { get; set; }

        /// <summary>
        /// 用途位置
        /// </summary>
        public virtual string? Purpose { get; set; }

        /// <summary>
        /// 刀具状态
        /// </summary>
        public virtual string? CutterStatus { get; set; }
    }
}
