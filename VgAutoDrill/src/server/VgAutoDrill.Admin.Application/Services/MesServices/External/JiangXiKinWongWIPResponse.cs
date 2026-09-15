namespace VgAutoDrill.Admin.Application.Services.MesServices.External
{
    internal class JiangXiKinWongWIPResponse
    {
        public string ContainerName { get; set; }
        public string ProductName { get; set; }
        public string StackNum { get; set; }
        public string PnlQty { get; set; }

        public string ShipDate { get; set; }
        public string SpecGroup { get; set; }

        /// <summary>
        /// 钻带资料
        /// </summary>
        public string Param01 { get; set; }

        public string LayerNum { get; set; }

        /// <summary>
        /// 是否在钻孔计划仓
        /// 当前仓位;钻孔计划仓位。示例：J03-SC-OUT-AGV;J03-DR-IN-Schedule
        /// </summary>
        public string Param02 { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public string Param03 { get; set; }
        /// <summary>
        /// 涨缩标识
        /// </summary>
        public string Param04 { get; set; }
        /// <summary>
        /// 预计时长
        /// </summary>
        public string Param05 { get; set; }
        /// <summary>
        /// 计划放钻孔孔数
        /// </summary>
        public string Param06 { get; set; }
        /// <summary>
        /// 板长
        /// </summary>
        public string Param07 { get; set; }
        /// <summary>
        /// 配刀相关内容
        /// </summary>
        public string Param08 { get; set; }
        /// <summary>
        /// 是否紧急下料，Y-时，熟料不缓存，尽快转运到拆PIN
        /// </summary>
        public string Param09 { get; set; }
    }
}
