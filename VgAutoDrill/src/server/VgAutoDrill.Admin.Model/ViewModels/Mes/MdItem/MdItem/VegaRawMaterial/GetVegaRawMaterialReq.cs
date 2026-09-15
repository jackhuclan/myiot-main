namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial
{
    public class GetVegaRawMaterialReq
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string? ItemCode { get; set; }


        /// <summary>
        /// 1、线边仓 2、转入途中 3、中转位 4、AGV
        /// </summary>
        public int Location { get; set; }

    }
}
